USE WeightApp
GO

DROP PROCEDURE IF EXISTS usp_GetUserDetails
GO

DROP PROCEDURE IF EXISTS usp_RegisterUser
GO

CREATE PROCEDURE usp_RegisterUser(@Email varchar(100), @Password varchar(100), @Name varchar(30), @IsMale bit, @DateOfBirth date)
AS BEGIN
    SET NOCOUNT ON

    if exists(select 1 from AppUsers where Email = @Email)
    begin
        THROW 51000, 'Email is used', 1
    end

    insert into AppUsers(Email, Password, Name, IsMale, DateOfBirth)
    values (@Email, @Password, @Name, @IsMale, @DateOfBirth)

    select UserID, Email, Password, Name, IsMale, DateOfBirth
    from AppUsers
    where Email = @Email
END
GO

DROP PROCEDURE IF EXISTS usp_LoginUser
GO

CREATE PROCEDURE usp_LoginUser(@Email varchar(100), @Password varchar(100))
AS BEGIN
    select UserID, Email, Password, Name, IsMale, DateOfBirth
    from AppUsers
    where Email = @Email and Password = @Password
END
GO

DROP PROCEDURE IF EXISTS usp_UpdateUserDetails
GO

CREATE PROCEDURE usp_UpdateUserDetails(@UserID int, @Password varchar(100), @Name varchar(30), @IsMale bit, @DateOfBirth date)
AS BEGIN
    SET NOCOUNT ON

    update AppUsers
    set
        Name = @Name,
        Password = @Password,
        IsMale = @IsMale,
        DateOfBirth = @DateOfBirth
    where UserID = @UserID

    SET NOCOUNT OFF

    select UserID, Email, Password, Name, IsMale, DateOfBirth
    from AppUsers
    where UserID = @UserID
END
GO

DROP PROCEDURE IF EXISTS usp_RemoveUser
GO

CREATE PROCEDURE usp_RemoveUser(@UserID int)
AS BEGIN
    SET NOCOUNT ON

    delete mh
    from MealHistory mh
        inner join AppUserGoals aug on mh.GoalID = aug.GoalID
    where aug.UserID = @UserID

    delete from Products
    where UserID = @UserID

    delete gp
    from GoalProgress gp
        inner join AppUserGoals aug on gp.GoalID = aug.GoalID
    where aug.UserID = @UserID

    delete from AppUserGoals
    where UserID = @UserID

    SET NOCOUNT OFF

    delete AppUsers
    where UserID = @UserID
END
GO