USE WeightApp
GO

DROP PROCEDURE IF EXISTS usp_GetProducts
GO

CREATE PROCEDURE usp_GetProducts(@UserID int)
AS BEGIN
    select
        p.ProductID,
        p.UserID,
        p.Name,
        p.Calories,
        p.Carbohydrates,
        p.Proteins,
        p.Fats,
        pc.CategoryID,
        pc.Name
    from Products p
        inner join ProductCategories pc on p.CategoryID = pc.CategoryID
    where UserID is NULL
    UNION
    select
        p.ProductID,
        p.UserID,
        p.Name,
        p.Calories,
        p.Carbohydrates,
        p.Proteins,
        p.Fats,
        pc.CategoryID,
        pc.Name
    from Products p
        inner join ProductCategories pc on p.CategoryID = pc.CategoryID
    where UserID = @UserID
END
GO

DROP PROCEDURE IF EXISTS usp_AddProduct
GO

CREATE PROCEDURE usp_AddProduct(@UserID int, @Name varchar(50), @CategoryID int, @Calories int, @Carbohydrates int, @Proteins int, @Fats int)
AS BEGIN
    SET NOCOUNT  ON

    insert into Products(UserID, Name, CategoryID, Calories, Carbohydrates, Proteins, Fats)
    values (@UserID, @Name, @CategoryID, @Calories, @Carbohydrates, @Proteins, @Fats)

    SET NOCOUNT OFF

    select
        p.ProductID,
        p.UserID,
        p.Name,
        p.Calories,
        p.Carbohydrates,
        p.Proteins,
        p.Fats,
        pc.CategoryID,
        pc.Name
    from Products p
        inner join ProductCategories pc on p.CategoryID = pc.CategoryID
    where p.ProductID = SCOPE_IDENTITY()
END
GO

DROP PROCEDURE IF EXISTS usp_RemoveProduct
GO

CREATE PROCEDURE usp_RemoveProduct(@UserID int, @ProductID int)
AS BEGIN
    if exists(select 1 from MealHistory where ProductID = @ProductID)
    begin
        THROW 51009, 'Cant remove product', 1
    end

    delete from Products
    where ProductID = @ProductID and UserID = @UserID
END
GO