use WeightApp
GO

DROP VIEW IF EXISTS vwProductsByMealType
GO

CREATE VIEW vwProductsByMealType
AS
    select
        [Product] = p.Name,
        bmt.Comsumed,
        bmt.Calories,
        mt.Name
    from Products p
        right outer join (
            select
                mh.ProductID,
                mt.MealTypeID,
                [Comsumed] = COUNT(mh.ProductID),
                [Calories] = SUM(mh.Amount)
            from MealTypes mt
                left outer join MealHistory mh on mh.MealTypeID = mt.MealTypeID
            group by mt.MealTypeID, mh.ProductID
        ) bmt on bmt.ProductID = p.ProductID
        inner join MealTypes mt on mt.MealTypeID = bmt.MealTypeID
GO


DROP VIEW IF EXISTS vwAverageCaloriesByMealType
GO

CREATE VIEW vwAverageCaloriesByMealType
AS
    select *
    from (
        select
            [MealType] = mt.Name,
            [Product] = p.Name,
            [Amount] = mh.Amount
        from MealTypes mt
             left outer join MealHistory mh on mt.MealTypeID = mh.MealTypeID
             left outer join Products p on mh.ProductID = p.ProductID
    ) x
    pivot (AVG(x.Amount) for x.MealType in ([Breakfast], [Brunch], [Lunch], [Supper])) as AvgCalloriesPerMealType
GO