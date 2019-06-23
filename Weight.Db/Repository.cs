

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

namespace WeightApp.Db
{
    public interface IRepository
    {
        Task<UserEntity> LoginUser(string email, string password);
        Task<UserEntity> RegisterUser(UserEntity user);
        Task RemoveUser(int userId);
        Task<UserEntity> UpdateDetails(UserEntity user);
        Task<IEnumerable<ProductCategoryEntity>> GetAllCategories();
        Task<IEnumerable<MealTypeEntity>> GetMealTypes();
        Task<IEnumerable<ProductEntity>> GetProducts(int userId);
        Task RemoveProduct(int productId, int userId);
        Task<ProductEntity> AddProduct(ProductEntity product);
        Task<IEnumerable<GoalEntity>> GetGoals(int userId, int? goalId);
        Task<GoalEntity> CompleteGoal(int goalId, int userId, DateTime completeDate);
        Task DeleteGoal(int goalId, int userId);
        Task<GoalEntity> SetNewGoal(GoalEntity goal);
        Task<IEnumerable<MealInstanceEntity>> GetMealsForGoal(int goalId, int userId);
        Task<MealInstanceEntity> AddMeal(int userId, MealInstanceEntity entity);
        Task RemoveMeal(int mealInstanceId, int userId);
        Task<IEnumerable<GoalProgressEntity>> GetProgress(int userId, bool currentProgressOnly = true);
        Task<GoalProgressEntity> TrackProgress(int userId, GoalProgressEntity entity);
        Task RemoveProgress(int progressId, int userId);
        Task<IEnumerable<MealInstanceEntity>> GetMealsForDate(int userId, DateTime date);
        Task<int> GetMealTotalsForDate(int userId, DateTime date);
        Task<IEnumerable<MealTotalsEntity>> GetMealTotalsForGoal(int userId, int goalId);
    }

    public class Repository : IRepository
    {
        private Func<IDbConnection> _connectionFactory;

        private IHashProvider _hashProvider;

        public Repository(Func<IDbConnection> connectionFactory, IHashProvider hashProvider)
        {
            if (connectionFactory == null)
                throw new ArgumentNullException(nameof(connectionFactory));

            if (hashProvider == null)
                throw new ArgumentNullException(nameof(hashProvider));


            _connectionFactory = connectionFactory;
            _hashProvider = hashProvider;
        }

        public async Task<UserEntity> LoginUser(string email, string password)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    cnn.Open();

                    var user = await cnn.QueryFirstOrDefaultAsync<UserEntity>(
                        "usp_LoginUser",
                        new { Email = email, Password = _hashProvider.GetHash(password) },
                        commandType: CommandType.StoredProcedure
                        );

                    if (user == null)
                    {
                        throw new DbException("Login failed");
                    }

                    return user;
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }

                return null;
            }
        }

        public async Task<UserEntity> RegisterUser(UserEntity user)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    cnn.Open();

                    var newUser = await cnn.QueryFirstOrDefaultAsync<UserEntity>(
                        "usp_RegisterUser",
                        new
                        {
                            user.Email,
                            Password = _hashProvider.GetHash(user.Password),
                            user.Name,
                            user.IsMale,
                            user.DateOfBirth
                        },
                        commandType: CommandType.StoredProcedure
                        );

                    if (newUser == null)
                    {
                        throw new DbException("Failed to add user");
                    }

                    return newUser;
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task RemoveUser(int userId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    cnn.Open();

                    var rows = await cnn.ExecuteAsync(
                        "usp_RemoveUser",
                        new { UserID = userId },
                        commandType: CommandType.StoredProcedure
                        );

                    if (rows == 0)
                    {
                        throw new DbException("Failed to remove user");
                    }
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }
        }

        public async Task<UserEntity> UpdateDetails(UserEntity user)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    cnn.Open();

                    var newUser = await cnn.QueryFirstOrDefaultAsync<UserEntity>(
                        "usp_UpdateUserDetails",
                        new { UserID = user.UserId, user.Password, user.Name, user.IsMale, user.DateOfBirth },
                        commandType: CommandType.StoredProcedure
                        );

                    if (newUser == null)
                    {
                        throw new DbException("Failed to update user details");
                    }
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }

                return null;
            }
        }

        public async Task<IEnumerable<ProductCategoryEntity>> GetAllCategories()
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    cnn.Open();

                    return await cnn.QueryAsync<ProductCategoryEntity>(
                        "usp_GetAllCategories",
                        null,
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }

                return null;
            }
        }

        public async Task<IEnumerable<MealTypeEntity>> GetMealTypes()
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QueryAsync<MealTypeEntity>(
                        "usp_GetMealTypes",
                        null,
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task<IEnumerable<ProductEntity>> GetProducts(int userId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    var result = await cnn.QueryAsync<ProductEntity, ProductCategoryEntity, ProductEntity>(
                        "usp_GetProducts",
                        (prod, cat) =>
                        {
                            prod.ProductCategory = cat;
                            return prod;
                        },
                        splitOn: "CategoryID",
                        param: new { UserId = userId },
                        commandType: CommandType.StoredProcedure
                        );
                    return result.ToList();
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task RemoveProduct(int productId, int userId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    await cnn.ExecuteAsync(
                        "usp_RemoveProduct",
                        new { ProductID = productId, UserID = userId },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }
        }

        public async Task<ProductEntity> AddProduct(ProductEntity product)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    var newProduct = await cnn.QueryAsync<ProductEntity, ProductCategoryEntity, ProductEntity>(
                        "usp_AddProduct",
                        (prod, cat) =>
                        {
                            prod.ProductCategory = cat;
                            return prod;
                        },
                        splitOn: "CategoryID",
                        param: new
                        {
                            product.UserId,
                            product.Name,
                            product.ProductCategory.CategoryId,
                            product.Calories,
                            product.Carbohydrates,
                            product.Proteins,
                            product.Fats
                        },
                        commandType: CommandType.StoredProcedure
                        );

                    return newProduct.First();
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }

                return null;
            }
        }

        public async Task<IEnumerable<GoalEntity>> GetGoals(int userId, int? goalId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QueryAsync<GoalEntity>(
                        "usp_GetGoals",
                        new { userId, goalId },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task<GoalEntity> CompleteGoal(int goalId, int userId, DateTime completeDate)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QuerySingleOrDefaultAsync<GoalEntity>(
                        "usp_CompleteGoal",
                        new { userId, goalId, completeDate },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task DeleteGoal(int goalId, int userId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    await cnn.ExecuteAsync(
                        "usp_DeleteGoal",
                        new { userId, goalId },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }
        }

        public async Task<GoalEntity> SetNewGoal(GoalEntity goal)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QuerySingleAsync<GoalEntity>(
                        "usp_SetNewGoal",
                        new { goal.UserId, goal.StartDate, goal.Weight, goal.WeightGoal, goal.Height },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task<IEnumerable<MealInstanceEntity>> GetMealsForGoal(int goalId, int userId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QueryAsync<MealInstanceEntity, ProductEntityRef, MealTypeEntity, GoalEntity, MealInstanceEntity>(
                        "usp_GetMealsForGoal",
                        (mie, per, mte, goal) =>
                        {
                            mie.Product = per;
                            mie.MealType = mte;
                            mie.Goal = goal;
                            return mie;
                        },
                        splitOn: "ProductID,MealTypeID,GoalID",
                        param: new { goalId, userId },
                        commandType:CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task<IEnumerable<MealInstanceEntity>> GetMealsForDate(int userId, DateTime date)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QueryAsync<MealInstanceEntity, ProductEntityRef, MealTypeEntity, GoalEntity, MealInstanceEntity>(
                        "usp_GetMealsForDate",
                        (mie, per, mte, goal) =>
                        {
                            mie.Product = per;
                            mie.MealType = mte;
                            mie.Goal = goal;
                            return mie;
                        },
                        splitOn: "ProductID,MealTypeID,GoalID",
                        param: new { date, userId },
                        commandType:CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task<MealInstanceEntity> AddMeal(int userId, MealInstanceEntity entity)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    var result = await cnn.QueryAsync<MealInstanceEntity, ProductEntityRef, MealTypeEntity, GoalEntity, MealInstanceEntity>(
                        "usp_AddMeal",
                        (mie, per, mte, goal) =>
                        {
                            mie.Product = per;
                            mie.MealType = mte;
                            mie.Goal = goal;
                            return mie;
                        },
                        splitOn: "ProductID,MealTypeID,GoalID",
                        param: new
                        {
                            entity.MealType.MealTypeId, 
                            entity.Product.ProductId, 
                            entity.Goal.GoalId, 
                            entity.Amount, 
                            entity.Date,
                            userId
                        },
                        commandType: CommandType.StoredProcedure
                        );

                    return result.FirstOrDefault();
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task RemoveMeal(int mealInstanceId, int userId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    await cnn.ExecuteAsync(
                        "usp_RemoveMeal",
                        new { userId, mealInstanceId },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }
        }

        public async Task<IEnumerable<GoalProgressEntity>> GetProgress(int userId, bool currentProgressOnly = true)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QueryAsync<GoalProgressEntity>(
                        "usp_GetProgress",
                        new { userId, currentProgressOnly },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task<GoalProgressEntity> TrackProgress(int userId, GoalProgressEntity entity)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QueryFirstOrDefaultAsync<GoalProgressEntity>(
                        "usp_TrackProgress",
                        new { userId, entity.GoalId, entity.Weight, entity.Date },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        public async Task RemoveProgress(int progressId, int userId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    await cnn.ExecuteAsync(
                        "usp_RemoveProgress",
                        new { userId, progressId },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }
        }

        public async Task<int> GetMealTotalsForDate(int userId, DateTime date)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.ExecuteScalarAsync<int>(
                        "usp_GetMealsTotalsForDate",
                        new { userId, date },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return 0;
        }

        public async Task<IEnumerable<MealTotalsEntity>> GetMealTotalsForGoal(int userId, int goalId)
        {
            using (var cnn = _connectionFactory())
            {
                try
                {
                    return await cnn.QueryAsync<MealTotalsEntity>(
                        "usp_GetMealsTotalsForGoal",
                        new { userId, goalId },
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (SqlException ex)
                {
                    if (HandleDbException(ex))
                        throw;
                }
            }

            return null;
        }

        private static bool HandleDbException(SqlException ex)
        {
            if (ex.Number >= 51000 && ex.Number < 52000)
            {
                var dbex = new DbException(ex.Message, ex);
                dbex.Code = ex.Number;
                throw dbex;
            }

            return true;
        }
    }
}