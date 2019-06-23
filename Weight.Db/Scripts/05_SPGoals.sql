use WeightApp
GO

DROP PROCEDURE IF EXISTS usp_GetGoals
GO

CREATE PROCEDURE usp_GetGoals(@UserID int, @GoalID int = NULL)
AS BEGIN
    select
        GoalID,
        UserID,
        StartDate,
        Weight,
        WeightGoal,
        Height,
        IsCompleted,
        CompleteDate,
        [IsSuccess] = (case when (Weight <= WeightGoal) then 1 else 0 end)
    from AppUserGoals
    where UserID = @UserID and (@GoalID is NULL or GoalID = @GoalID)
END
GO

DROP PROCEDURE IF EXISTS usp_SetNewGoal
GO

CREATE PROCEDURE usp_SetNewGoal(@UserID int, @StartDate datetime, @Weight int, @WeightGoal int, @Height int)
AS BEGIN
    SET NOCOUNT  ON

    if exists(select 1 from AppUserGoals where UserID = @UserID and IsCompleted = 0)
    begin
        THROW 51005, 'Cant start new goal until previous is not completed', 1
    end

    if @Weight <= @WeightGoal
    begin
        THROW 51006, 'Weight goal cant be less than current weight', 1
    end

    insert into AppUserGoals (UserID, StartDate, Weight, WeightGoal, Height, IsCompleted)
    values (@UserID, @StartDate, @Weight, @WeightGoal, @Height, 0)

    SET NOCOUNT OFF

    select
        GoalID,
        UserID,
        StartDate,
        Weight,
        WeightGoal,
        Height,
        IsCompleted,
        CompleteDate,
        [IsSuccess] = (case when (Weight <= WeightGoal) then 1 else 0 end)
    from AppUserGoals
    where GoalID = SCOPE_IDENTITY()
END
GO

DROP PROCEDURE IF EXISTS usp_CompleteGoal
GO

CREATE PROCEDURE usp_CompleteGoal(@GoalID int, @UserID int, @CompleteDate datetime)
AS BEGIN
    SET NOCOUNT ON

    update AppUserGoals
    set
        IsCompleted = 1,
        CompleteDate = @CompleteDate
    where GoalID = @GoalID and UserID = @UserID

    SET NOCOUNT OFF

    exec usp_GetGoals @UserID = @UserID, @GoalID = @GoalID
END
GO

DROP PROCEDURE IF EXISTS usp_DeleteGoal
GO

CREATE PROCEDURE usp_DeleteGoal(@GoalID int, @UserID int)
AS BEGIN
    delete from MealHistory
    where GoalID = @GoalID

    delete from GoalProgress
    where GoalID = @GoalID

    delete from AppUserGoals
    where GoalID = @GoalID and UserID = @UserID
END
GO

DROP PROCEDURE IF EXISTS usp_GetProgress
GO

CREATE PROCEDURE usp_GetProgress(@UserID int, @CurrentProgressOnly bit)
AS BEGIN
    select
        gp.ProgressID,
        gp.GoalID,
        gp.Weight,
        gp.Date
    from GoalProgress gp
        inner join AppUserGoals aug on gp.GoalID = aug.GoalID
    where aug.UserID = @UserID and (@CurrentProgressOnly = 0 or (@CurrentProgressOnly = 1 and aug.IsCompleted = 0))
END
GO

DROP PROCEDURE IF EXISTS usp_TrackProgress
GO

CREATE PROCEDURE usp_TrackProgress(@UserID int, @GoalID int, @Weight int, @Date datetime)
AS BEGIN
    SET NOCOUNT  ON

    if not exists(
        select 1
        from AppUserGoals p
        where p.UserID = @UserID and p.GoalID = @GoalID)
    begin
        THROW 50100, 'Forbidden', 1
    end

    insert into GoalProgress (GoalID, Weight, Date)
    values (@GoalID, @Weight, @Date)

    SET NOCOUNT  OFF

    select
        gp.ProgressID,
        gp.GoalID,
        gp.Weight,
        gp.Date
    from GoalProgress gp
    where gp.ProgressID = SCOPE_IDENTITY()
END
GO

DROP PROCEDURE IF EXISTS usp_RemoveProgress
GO

CREATE PROCEDURE usp_RemoveProgress(@ProgressID int, @UserID int)
AS BEGIN
    if not exists(
        select 1
        from GoalProgress p
            inner join AppUserGoals aug on p.GoalID = aug.GoalID
        where aug.UserID = @UserID and p.ProgressID = @ProgressID)
    begin
        THROW 50100, 'Forbidden', 1
    end

    if exists(
        select 1
        from GoalProgress gp
            inner join AppUserGoals aug on gp.GoalID = aug.GoalID
        where gp.ProgressID = @ProgressID and aug.IsCompleted = 1)
    begin
        THROW 51007, 'Cant remove progress for completed goal', 1
    end

    delete from GoalProgress
    where ProgressID = @ProgressID
END
GO

DROP PROCEDURE IF EXISTS usp_GetMeal
GO

CREATE PROCEDURE usp_GetMeal(@MealInstanceId int, @UserID int)
AS BEGIN
    if not exists(
        select 1
        from AppUserGoals aug
            inner join MealHistory m on aug.GoalID = m.GoalID
        where aug.UserID = @UserID and m.MealInstanceID = @MealInstanceID)
    begin
        THROW 50100, 'Forbidden', 1
    end

    select
        mh.MealInstanceID,
        mh.Amount,
        mh.Date,
        p2.ProductID,
        p2.Name,
        m.MealTypeID,
        m.Name,
        aug.GoalID,
        aug.UserID,
        aug.StartDate,
        aug.Weight,
        aug.WeightGoal,
        aug.Height,
        aug.IsCompleted,
        aug.CompleteDate
    from MealHistory mh
        inner join MealTypes m on mh.MealTypeID = m.MealTypeID
        inner join Products p2 on mh.ProductID = p2.ProductID
        inner join AppUserGoals aug on mh.GoalID = aug.GoalID
    where mh.MealInstanceID = @MealInstanceId
END
GO

DROP PROCEDURE IF EXISTS usp_GetMealsForGoal
GO

CREATE PROCEDURE usp_GetMealsForGoal(@GoalID int, @UserID int)
AS BEGIN
    if not exists(select 1 from AppUserGoals aug where aug.GoalID = @GoalID and aug.UserID = @UserID)
    begin
        THROW 50100, 'Forbidden', 1
    end

    select
        mh.MealInstanceID,
        mh.Amount,
        mh.Date,
        p2.ProductID,
        p2.Name,
        m.MealTypeID,
        m.Name,
        aug.GoalID,
        aug.UserID,
        aug.StartDate,
        aug.Weight,
        aug.WeightGoal,
        aug.Height,
        aug.IsCompleted,
        aug.CompleteDate
    from MealHistory mh
        inner join MealTypes m on mh.MealTypeID = m.MealTypeID
        inner join Products p2 on mh.ProductID = p2.ProductID
        inner join AppUserGoals aug on mh.GoalID = aug.GoalID
    where mh.GoalID = @GoalID
END
GO

DROP PROCEDURE IF EXISTS usp_GetMealsForDate
GO

CREATE PROCEDURE usp_GetMealsForDate(@UserID int, @Date datetime)
AS BEGIN
    select
        mh.MealInstanceID,
        mh.Amount,
        mh.Date,
        p2.ProductID,
        p2.Name,
        m.MealTypeID,
        m.Name,
        aug.GoalID,
        aug.UserID,
        aug.StartDate,
        aug.Weight,
        aug.WeightGoal,
        aug.Height,
        aug.IsCompleted,
        aug.CompleteDate
    from MealHistory mh
        inner join AppUserGoals aug on mh.GoalID = aug.GoalID
        inner join MealTypes m on mh.MealTypeID = m.MealTypeID
        inner join Products p2 on mh.ProductID = p2.ProductID
    where aug.UserID = @UserID and DATEDIFF(dd, mh.Date, @Date) = 0
END
GO

DROP PROCEDURE IF EXISTS usp_GetMealsTotalsForGoal
GO

CREATE PROCEDURE usp_GetMealsTotalsForGoal(@GoalID int, @UserID int)
AS BEGIN
    if not exists(select 1 from AppUserGoals aug where aug.GoalID = @GoalID and aug.UserID = @UserID)
    begin
        THROW 50100, 'Forbidden', 1
    end

    select
        [Consumed] = SUM(r.Amount * r.Calories / 100),
        r.Date
    from (
        select
            mh.Amount,
            p2.Calories,
            [Date] = CONVERT(date, mh.Date)
        from MealHistory mh
            inner join AppUserGoals aug on mh.GoalID = aug.GoalID
            inner join Products p2 on mh.ProductID = p2.ProductID
        where mh.GoalID = @GoalID
    ) r
    group by r.Date
END
GO

DROP PROCEDURE IF EXISTS usp_GetMealsTotalsForDate
GO

CREATE PROCEDURE usp_GetMealsTotalsForDate(@UserID int, @Date datetime)
AS BEGIN
    select
        [Consumed] = SUM(mh.Amount * p2.Calories / 100)
    from MealHistory mh
        inner join AppUserGoals aug on mh.GoalID = aug.GoalID
        inner join Products p2 on mh.ProductID = p2.ProductID
    where aug.UserID = @UserID and DATEDIFF(dd, mh.Date, @Date) = 0
END
GO


DROP PROCEDURE IF EXISTS usp_AddMeal
GO

CREATE PROCEDURE usp_AddMeal(@MealTypeID int, @ProductID int, @UserID int, @GoalID int, @Amount int, @Date datetime)
AS BEGIN
    SET NOCOUNT ON
    if not exists(select 1 from AppUserGoals aug where aug.GoalID = @GoalID and aug.UserID = @UserID)
    begin
        THROW 50100, 'Forbidden', 1
    end

    if exists(
        select 1
        from MealHistory mh
            inner join AppUserGoals aug on mh.GoalID = aug.GoalID
        where aug.GoalID = @GoalID and aug.IsCompleted = 1)
    begin
        THROW 51008, 'Cant edit completed goal', 1
    end

    insert into MealHistory (MealTypeID, ProductID, GoalID, Amount, Date)
    values (@MealTypeID, @ProductID, @GoalID, @Amount, @Date)

    SET NOCOUNT OFF

    declare @id int = SCOPE_IDENTITY()
    exec usp_GetMeal @MealInstanceId = @id, @UserID = @UserID
END
GO

DROP PROCEDURE IF EXISTS usp_RemoveMeal
GO

CREATE PROCEDURE usp_RemoveMeal(@MealInstanceID int, @UserID int)
AS BEGIN
    if not exists(
        select 1
        from AppUserGoals aug
            inner join MealHistory m on aug.GoalID = m.GoalID
        where aug.UserID = @UserID and m.MealInstanceID = @MealInstanceID)
    begin
        THROW 50100, 'Forbidden', 1
    end

    if exists(
        select 1
        from MealHistory mh
            inner join AppUserGoals aug on mh.GoalID = aug.GoalID
        where mh.MealInstanceID = @MealInstanceID and aug.IsCompleted = 1)
    begin
        THROW 51008, 'Cant edit completed goal', 1
    end

    delete from MealHistory
    where MealInstanceID = @MealInstanceID
END
GO