use WeightApp
GO

insert into ProductCategories(CategoryID, Name)
values (1, 'CategoryA')
insert into ProductCategories(CategoryID, Name)
values (2, 'CategoryB')
insert into ProductCategories(CategoryID, Name)
values (3, 'CategoryC')
GO

insert into MealTypes(MealTypeID, Name)
values (1, 'Breakfast')
insert into MealTypes(MealTypeID, Name)
values (2, 'Brunch')
insert into MealTypes(MealTypeID, Name)
values (3, 'Lunch')
insert into MealTypes(MealTypeID, Name)
values (4, 'Supper')
insert into MealTypes(MealTypeID, Name)
values (5, 'Snack')
GO

insert into Products(UserID, Name, CategoryID, Calories, Carbohydrates, Proteins, Fats)
values (NULL, 'ProductA', 1, 100, 200, 300, 400)
insert into Products(UserID, Name, CategoryID, Calories, Carbohydrates, Proteins, Fats)
values (NULL, 'ProductB', 2, 200, 210, 310, 410)
insert into Products(UserID, Name, CategoryID, Calories, Carbohydrates, Proteins, Fats)
values (NULL, 'ProductC', 3, 300, 220, 320, 420)
insert into Products(UserID, Name, CategoryID, Calories, Carbohydrates, Proteins, Fats)
values (NULL, 'ProductD', 1, 400, 230, 330, 430)
insert into Products(UserID, Name, CategoryID, Calories, Carbohydrates, Proteins, Fats)
values (NULL, 'ProductE', 1, 500, 240, 340, 440)
GO

insert into AppUsers (Email, Password, Name, IsMale, DateOfBirth)
values ('user@example.com', '148de9c5a7a44d19e56cd9ae1a554bf67847afb0c58f6e12fa29ac7ddfca9940', 'Me', 1, '2019-06-16')
GO