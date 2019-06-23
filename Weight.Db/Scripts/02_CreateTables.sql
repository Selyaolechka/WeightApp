USE WeightApp
GO

DROP TABLE IF EXISTS MealHistory
GO
DROP TABLE IF EXISTS Products
GO
DROP TABLE IF EXISTS MealTypes
GO
DROP TABLE IF EXISTS ProductCategories
GO
DROP TABLE IF EXISTS GoalProgress
GO
DROP TABLE IF EXISTS AppUserGoals
GO
DROP TABLE IF EXISTS AppUsers
GO

CREATE TABLE AppUsers (
    UserID      int             NOT NULL IDENTITY(1, 1),
    Email       varchar(100)    NOT NULL,
    Password    varchar(100)    NOT NULL,
    Name        varchar(20)     NOT NULL,
    IsMale      bit             NOT NULL,
    DateOfBirth date            NOT NULL,
    CONSTRAINT PK_User PRIMARY KEY (UserID)
)
GO

CREATE TABLE AppUserGoals (
    GoalID          int         NOT NULL IDENTITY(1, 1),
    UserID          int         NOT NULL,
    StartDate       datetime    NOT NULL,
    Weight          int         NOT NULL,
    WeightGoal      int         NOT NULL,
    Height          int         NOT NULL,
    IsCompleted     bit         NOT NULL,
    CompleteDate    datetime    NULL
    CONSTRAINT PK_AppUserGoals PRIMARY KEY (GoalID),
    CONSTRAINT FK_AppUserGoals_AppUsers FOREIGN KEY (UserID) REFERENCES AppUsers(UserID)
)
GO

CREATE TABLE GoalProgress(
    ProgressID  int         NOT NULL IDENTITY(1, 1),
    GoalID      int         NOT NULL,
    Weight      int         NOT NULL,
    Date        datetime    NOT NULL,
    CONSTRAINT PK_GoalProgress PRIMARY KEY (ProgressID),
    CONSTRAINT FK_GoalProgress_AppUserGoals FOREIGN KEY (GoalID) REFERENCES AppUserGoals(GoalID)
)
GO

CREATE TABLE ProductCategories (
    CategoryID  int         NOT NULL,
    Name        varchar(50) NOT NULL,
    CONSTRAINT PK_ProductCategories PRIMARY KEY (CategoryID)
)
GO

CREATE TABLE Products (
    ProductID       int         NOT NULL IDENTITY(1, 1),
    UserID          int         NULL,
    Name            varchar(50) NOT NULL,
    CategoryID      int         NULL,
    Calories        int         NOT NULL,
    Carbohydrates   int         NOT NULL,
    Proteins        int         NOT NULL,
    Fats            int         NOT NULL,
    CONSTRAINT PK_Products PRIMARY KEY (ProductID),
    CONSTRAINT FK_Products_AppUsers FOREIGN KEY (UserID) REFERENCES AppUsers(UserID),
    CONSTRAINT FK_Products_ProductCategories FOREIGN KEY (CategoryID) REFERENCES ProductCategories(CategoryID)
)
GO

CREATE TABLE MealTypes (
    MealTypeID  int         NOT NULL,
    Name        varchar(20) NOT NULL,
    CONSTRAINT PK_MealTypes PRIMARY KEY (MealTypeID)
)
GO

CREATE TABLE MealHistory (
    MealInstanceID  int         NOT NULL IDENTITY(1, 1),
    MealTypeID      int         NOT NULL,
    ProductID       int         NOT NULL,
    GoalID          int         NOT NULL,
    Amount          int         NOT NULL,
    Date            datetime    NOT NULL,
    CONSTRAINT PK_MealHistory PRIMARY KEY (MealInstanceID),
    CONSTRAINT FK_MealHistory_MealTypes FOREIGN KEY (MealTypeID) REFERENCES MealTypes(MealTypeID),
    CONSTRAINT FK_MealHistory_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    CONSTRAINT FK_MealHistory_AppUserGoals FOREIGN KEY (GoalID) REFERENCES AppUserGoals(GoalID)
)
GO