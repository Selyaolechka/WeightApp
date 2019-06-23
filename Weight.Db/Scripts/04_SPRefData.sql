use WeightApp
GO

DROP PROCEDURE IF EXISTS usp_GetAllCategories
GO

CREATE PROCEDURE usp_GetAllCategories
AS BEGIN
    select
        CategoryID,
        Name
    from ProductCategories
END
GO

DROP PROCEDURE IF EXISTS usp_AddProductCategory
GO

CREATE PROCEDURE usp_AddProductCategory(@CategoryID int, @Name varchar(50))
AS BEGIN
    if exists(select 1 from ProductCategories where Name = @Name)
    begin
        THROW 51001, 'Category with same name exists', 1
    end

    insert into ProductCategories(CategoryID, Name)
    values(@CategoryID, @Name)
END
GO

DROP PROCEDURE IF EXISTS usp_UpdateProductCategory
GO

CREATE PROCEDURE usp_UpdateProductCategory(@CategoryID int, @Name varchar(50))
AS BEGIN
    if @CategoryID = 1
    begin
        THROW 51002, 'Cant edit predefined category', 1
    end

    update ProductCategories
    set
        Name = @Name
    where CategoryID = @CategoryID
END
GO

DROP PROCEDURE IF EXISTS usp_DeleteProductCategory
GO

CREATE PROCEDURE usp_DeleteProductCategory(@CategoryID int)
AS BEGIN
    if @CategoryID = 1
    begin
        THROW 51003, 'Cant delete predefined category', 1
    end

    if exists(select 1 from Products where CategoryID = @CategoryID)
    begin
        THROW 51004, 'Cant delete category because it has associated products', 1
    end

    delete from ProductCategories
    where CategoryID = @CategoryID
END
GO

DROP PROCEDURE IF EXISTS usp_GetMealTypes
GO

CREATE PROCEDURE usp_GetMealTypes
AS BEGIN
    select
        MealTypeID,
        Name
    from MealTypes
END
GO
