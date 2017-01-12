using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SQLite.Net;

namespace WODTasticMobile
{
    public class SQL
    {
        readonly object dbLock = new object();
        const string DBClauseSyncOff = "PRAGMA SYNCHRONOUS=OFF;";
        const string DBClauseVacuum = "VACUUM;";

        #region Setup

        public bool SetupDB()
        {
            lock (dbLock)
            {
                try
                {
                    using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                    {
                        sqlcon.CreateTable<Accessory_Categories>();
                        sqlcon.CreateTable<Accessory_Work>();
                        sqlcon.CreateTable<Benchmark_Wods>();
                        sqlcon.CreateTable<Category>();
                        sqlcon.CreateTable<Category_Movements>();
                        sqlcon.CreateTable<Coach_Ind_Signup>();
                        sqlcon.CreateTable<Coach_Mass_Signup>();
                        sqlcon.CreateTable<Crossfit_Games>();
                        sqlcon.CreateTable<Crossfit_Liftoff>();
                        sqlcon.CreateTable<Crossfit_Masters>();
                        sqlcon.CreateTable<Crossfit_Gym_Signedup>();
                        sqlcon.CreateTable<Crossfit_Regionals>();
                        sqlcon.CreateTable<Crossfit_Open>();
                        sqlcon.CreateTable<Crossfit_Open_Workouts>();
                        sqlcon.CreateTable<Distance>();
                        sqlcon.CreateTable<DOB>();
                        sqlcon.CreateTable<Expiration>();
                        sqlcon.CreateTable<Gender>();
                        sqlcon.CreateTable<Girl_Benchmarks>();
                        sqlcon.CreateTable<Hero_Wods>();
                        sqlcon.CreateTable<Max_Reps_Numbers>();
                        sqlcon.CreateTable<Movements>();
                        sqlcon.CreateTable<Minutes>();
                        sqlcon.CreateTable<Oly_Lifts>();
                        sqlcon.CreateTable<RM_Max_Reps>();
                        sqlcon.CreateTable<RXScaled>();
                        sqlcon.CreateTable<Strength_Oly_Lifts>();
                        sqlcon.CreateTable<User>();
                        sqlcon.CreateTable<Wod_By_Day>();
                        sqlcon.CreateTable<Wod_Number>();
                        sqlcon.CreateTable<Wod_Scores>();
                        sqlcon.CreateTable<WSM_Finals>();

                        sqlcon.Execute(DBClauseVacuum);
                    }
                    return true;
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        #endregion

        #region Delete

        public void DropATable(string table)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        string drop = string.Format("DELETE FROM {0}", table);
                        sqlcon.Execute(drop);
                        sqlcon.Commit();
                        sqlcon.Execute(DBClauseVacuum);
                    }
                    catch (Exception ex)
                    {
#if (DEBUG)
                        System.Diagnostics.Debug.WriteLine("Error in DropATable! {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void CleanUpDB()
        {
            lock (this.dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.Execute("DELETE FROM Accessory_Categories");
                        sqlcon.Execute("DELETE FROM Accessory_Work");
                        sqlcon.Execute("DELETE FROM Benchmark_Wods");
                        sqlcon.Execute("DELETE FROM Category");
                        sqlcon.Execute("DELETE FROM Category_Movements");
                        sqlcon.Execute("DELETE FROM Coach_Ind_Signup");
                        sqlcon.Execute("DELETE FROM Coach_Mass_Signup");
                        sqlcon.Execute("DELETE FROM Crossfit_Games");
                        sqlcon.Execute("DELETE FROM Crossfit_Liftoff");
                        sqlcon.Execute("DELETE FROM Crossfit_Masters");
                        sqlcon.Execute("DELETE FROM Crossfit_Gym_Signedup");
                        sqlcon.Execute("DELETE FROM Crossfit_Regionals");
                        sqlcon.Execute("DELETE FROM Crossfit_Open");
                        sqlcon.Execute("DELETE FROM Crossfit_Open_Workouts");
                        sqlcon.Execute("DELETE FROM Distance");
                        sqlcon.Execute("DELETE FROM DOB");
                        sqlcon.Execute("DELETE FROM Expiration");
                        sqlcon.Execute("DELETE FROM Gender");
                        sqlcon.Execute("DELETE FROM Girl_Benchmarks");
                        sqlcon.Execute("DELETE FROM Hero_Wods");
                        sqlcon.Execute("DELETE FROM Max_Reps_Numbers");
                        sqlcon.Execute("DELETE FROM Movements");
                        sqlcon.Execute("DELETE FROM Minutes");
                        sqlcon.Execute("DELETE FROM Oly_Lifts");
                        sqlcon.Execute("DELETE FROM RM_Max_Reps");
                        sqlcon.Execute("DELETE FROM RXScaled");
                        sqlcon.Execute("DELETE FROM Strength_Oly_Lifts");
                        sqlcon.Execute("DELETE FROM User");
                        sqlcon.Execute("DELETE FROM Wod_By_Day");
                        sqlcon.Execute("DELETE FROM Wod_Number");
                        sqlcon.Execute("DELETE FROM WodScores");
                        sqlcon.Execute("DELETE FROM WSM_Finals");
                        sqlcon.Commit();
                        sqlcon.Execute(DBClauseVacuum);
                    }
                    catch (Exception ex)
                    {
#if (DEBUG)
                        Debug.WriteLine("Error in CleanUpDB! {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        #endregion

        #region DeleteObject

        public void DeleteObject<T>(T imp) where T : IInterface
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        var command = string.Format("UPDATE {0} SET id={1} WHERE id={2}", typeof(T).ToString(), imp.id, imp.id);
                        sqlcon.Execute(command);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in DeleteObject - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        #endregion

        #region Setters

        public void AddOrUpdateExercise(List<Accessory_Categories> ac)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ac);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Accessory_Categories - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateExercise(Accessory_Categories ac)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Accessory_Categories SET id=?, " +
                            "accessory_category_id=?, accessory_category_name=? WHERE Id=?",
                                           ac.id, ac.accessory_category_id, ac.accessory_category_name, ac.id) == 0)
                            sqlcon.Insert(ac, typeof(Accessory_Categories));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Accessory_Categories - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateUsers(List<User> user)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(user);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateUsers - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateUsers(User i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE User SET id=?, user_id=?, " +
                            "user_firstname=?,user_lastname=?,user_name=?,user_password=?, user_email=?, " +
                            "name=?, started_date=?, inactive_date=? ,user_age=?," +
                                           "user_gender=?, user_dob=?, gym_id=?, admin_role=? WHERE id=?",
                                           i.user_id, i.user_id, i.user_firstname, i.user_lastname, i.user_name, i.user_password, i.user_email,
                                           i.name, i.started_date, i.inactive_date, i.user_age, i.user_gender, i.user_dob, i.gym_id,
                                           i.admin_role, i.id) == 0)
                            sqlcon.Insert(i, typeof(User));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateUsers - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateAccessoryWork(List<Accessory_Work> wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(wo);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Accessory_Work - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateAccessoryWork(Accessory_Work wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Accessory_Work SET id=?, " +
                            "accessory_work_id=?, accessory_name=?, accessory_category_name=? WHERE Id=?",
                                               wo.accessory_work_id, wo.accessory_name, wo.accessory_category_name, wo.id) == 0)
                            sqlcon.Insert(wo, typeof(Accessory_Work));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateWorkout - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateBenchmarkWods(List<Benchmark_Wods> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Benchmark_Wods - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateBenchmarkWods(Benchmark_Wods i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Benchmark_Wods SET id=?, " +
                            "benchmark_wods_id=?,benchmark_wods_name=?,benchmark_wods_wod=?,Time=?, Reps=?, " +
                            "User_Id=?, Comments=? WHERE id=?",
                                           i.id, i.benchmark_wods_id, i.benchmark_wods_name, i.benchmark_wods_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Benchmark_Wods));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateWodScores - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCategory(List<Category> wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(wo);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Category - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCategory(Category wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Category SET id=?, " +
                            "category_id=?, category_name=? WHERE id=?",
                                           wo.id, wo.category_id, wo.category_name, wo.id) == 0)
                            sqlcon.Insert(wo, typeof(Wod_By_Day));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Category - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCategoryMovements(List<Category_Movements> wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(wo);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Category_Movements - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCategoryMovements(Category_Movements wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Category_Movements SET id=?, " +
                                           "movement_id=?, category_movements=?, categories=? WHERE id=?",
                                           wo.id, wo.movement_id, wo.category_movements, wo.id) == 0)
                            sqlcon.Insert(wo, typeof(Category_Movements));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateOneRMMaxReps - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCoachIndSignup(List<Coach_Ind_Signup> wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(wo);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Coach_Ind_Signup - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateOlyLifts(Coach_Ind_Signup wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Coach_Ind_Signup SET id=?, " +
                            "coach_indiv_id=?, coach_indiv_address1=?, coach_indiv_city=?," +
                            "coach_indiv_state=?, coach_indiv_zip=?, coach_indiv_phone_number=?, coach_indiv_website=?," +
                            "coach_indiv_fb=?, admin_role=?, coach_indiv_email=?, coach_indiv_password=?," +
                            "started_date=?, inactive_date=? WHERE id=?",
                                           wo.id, wo.coach_indiv_id, wo.coach_indiv_address1, wo.coach_indiv_city,
                                           wo.coach_indiv_state, wo.coach_indiv_zip, wo.coach_indiv_phone_number,
                                           wo.coach_indiv_website, wo.coach_indiv_fb, wo.admin_role,
                                           wo.coach_indiv_email, wo.coach_indiv_password, wo.started_date,
                                           wo.inactive_date, wo.id) == 0)
                            sqlcon.Insert(wo, typeof(Coach_Ind_Signup));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateOlyLifts - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCoachMassSignup(List<Coach_Mass_Signup> wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(wo);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Coach_Mass_Signup - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCoachMassSignup(Coach_Mass_Signup wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Coach_Mass_Signup SET id=?, " +
                            "coach_mass_id=?, coach_mass_address1=?, coach_mass_citycity=?," +
                            "coach_mass_state=?, coach_mass_zip=?, coach_mass_phone_number=?, coach_mass_website=?," +
                            "coach_mass_fb=?, admin_role=?, coach_mass_email=?, coach_mass_password=?," +
                            "started_date=?, inactive_date=? WHERE id=?",
                                           wo.id, wo.coach_mass_id, wo.coach_mass_address1, wo.coach_mass_city,
                                           wo.coach_mass_state, wo.coach_mass_zip, wo.coach_mass_phone_number,
                                           wo.coach_mass_website, wo.coach_mass_fb, wo.admin_role,
                                           wo.coach_mass_email, wo.coach_mass_password, wo.started_date,
                                           wo.inactive_date, wo.id) == 0)
                            sqlcon.Insert(wo, typeof(Coach_Mass_Signup));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Coach_Mass_Signup - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitGames(List<Crossfit_Games> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Games - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitGames(Crossfit_Games i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Crossfit_Games SET id=?, " +
                            "crossfit_games_id=?,crossfit_games_event=?,crossfit_games_wod=? WHERE id=?",
                                           i.id, i.crossfit_games_id, i.crossfit_games_event, i.crossfit_games_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Crossfit_Games));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Games - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitGymSignedup(List<Crossfit_Gym_Signedup> wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(wo);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Gym_Signedup - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitGymSignedup(Crossfit_Gym_Signedup wo)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Crossfit_Gym_Signedup SET id=?, " +
                            "gym_id=?, gym_name=?, gym_address1=?," +
                            "gym_city=?, gym_zip=?, gym_phone_number=?, gym_website=?," +
                            "gym_fblink=?, gym_admin_role=?, gym_email=?, gym_password=?," +
                            "gym_started_date=?, gym_inactive_date=? WHERE id=?",
                                           wo.id, wo.gym_id, wo.gym_name, wo.gym_address1,
                                           wo.gym_city, wo.gym_state, wo.gym_zip,
                                           wo.gym_phone_number, wo.gym_website, wo.gym_fblink,
                                           wo.gym_admin_role, wo.gym_email, wo.gym_password, wo.gym_started_date,
                                           wo.gym_inactive_date, wo.id) == 0)
                            sqlcon.Insert(wo, typeof(Crossfit_Gym_Signedup));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Gym_Signedup - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitLiftoff(List<Crossfit_Liftoff> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Liftoff - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitLiftoff(Crossfit_Liftoff i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Crossfit_Liftoff SET id=?, " +
                            "crossfit_liftoff_workouts_id=?,crossfit_liftoff_workouts_name=?,crossfit_liftoff_workouts_wod=? WHERE id=?",
                                           i.id, i.crossfit_liftoff_workouts_id, i.crossfit_liftoff_workouts_name,
                                           i.crossfit_liftoff_workouts_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Crossfit_Liftoff));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Liftoff - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitMasters(List<Crossfit_Masters> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Masters - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitMasters(Crossfit_Masters i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Crossfit_Masters SET id=?, " +
                            "crossfit_masters_qualifying_id=?,crossfit_masters_qualifying_event=?,crossfit_masters_qualifying_wod=? WHERE id=?",
                                           i.id, i.crossfit_masters_qualifying_id, i.crossfit_masters_qualifying_event,
                                           i.crossfit_masters_qualifying_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Crossfit_Masters));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Masters - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitOpen(List<Crossfit_Open> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Open - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitOpen(Crossfit_Open i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Crossfit_Open SET id=?, " +
                            "crossfit_open_id=?,crossfit_open_name=?,crossfit_open_wod=? WHERE id=?",
                                           i.id, i.crossfit_open_id, i.crossfit_open_name,
                                           i.crossfit_open_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Crossfit_Open));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Open - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitOpenWorkouts(List<Crossfit_Open_Workouts> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Open_Workouts - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitOpenWorkouts(Crossfit_Open_Workouts i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Crossfit_Open_Workouts SET id=?, " +
                            "crossfit_open_workouts_id=?,crossfit_open_workouts_name=?,crossfit_open_workouts_wod=? WHERE id=?",
                                           i.id, i.crossfit_open_workouts_id, i.crossfit_open_workouts_name,
                                           i.crossfit_open_workouts_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Crossfit_Open_Workouts));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Open_Workouts - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitRegionals(List<Crossfit_Regionals> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Regionals - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateCrossfitRegionals(Crossfit_Regionals i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Crossfit_Regionals SET id=?, " +
                            "crossfit_regionals_id=?,crossfit_regionals_event=?,crossfit_regionals_wod=? WHERE id=?",
                                           i.id, i.crossfit_regionals_id, i.crossfit_regionals_event,
                                           i.crossfit_regionals_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Crossfit_Regionals));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Crossfit_Regionals - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateDistance(List<Distance> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Distance - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateDistance(Distance i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Distance SET id=?, " +
                            "distance_id=?,distance=? WHERE id=?",
                                           i.id, i.distance_id, i.distance, i.id) == 0)
                            sqlcon.Insert(i, typeof(Distance));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Distance - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateDOB(List<DOB> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in DOB - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateDOB(DOB i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE DOB SET id=?, " +
                            "dob_id=?,dob_month=?,dob_day=?,dob_year=? WHERE id=?",
                                           i.id, i.dob_id, i.dob_month,
                                           i.dob_day, i.dob_year, i.id) == 0)
                            sqlcon.Insert(i, typeof(DOB));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in DOB - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateExpiration(List<Expiration> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Expiration - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateExpiration(Expiration i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Expiration SET id=?, " +
                            "expiration_id=?,expiration_month=?,expiration_year=? WHERE id=?",
                                           i.id, i.expiration_id, i.expiration_month,
                                           i.expiration_year, i.id) == 0)
                            sqlcon.Insert(i, typeof(Expiration));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Expiration - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateGender(List<Gender> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Gender - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateGender(Gender i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Gender SET id=?, " +
                            "gender_id=?,gender=? WHERE id=?",
                                           i.id, i.gender_id, i.gender, i.id) == 0)
                            sqlcon.Insert(i, typeof(Gender));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Gender - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateGirlBenchmarks(List<Girl_Benchmarks> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Expiration - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateExpiration(Girl_Benchmarks i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Girl_Benchmarks SET id=?, " +
                            "benchmark_wods_id=?,benchmark_wods_name=?,benchmark_wods_wod=? WHERE id=?",
                                           i.id, i.benchmark_wods_id, i.benchmark_wods_name,
                                           i.benchmark_wods_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Girl_Benchmarks));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Girl_Benchmarks - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateHeroWods(List<Hero_Wods> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Hero_Wods - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateHeroWods(Hero_Wods i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Hero_Wods SET id=?, " +
                            "hero_wods_id=?,hero_wods_name=?,hero_wods_wod=? WHERE id=?",
                                           i.id, i.hero_wods_id, i.hero_wods_name,
                                           i.hero_wods_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Hero_Wods));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Hero_Wods - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateMaxRepsNumbers(List<Max_Reps_Numbers> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Max_Reps_Numbers - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateMaxRepsNumbers(Max_Reps_Numbers i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Max_Reps_Numbers SET id=?, " +
                            "max_reps_numbers_id=?,max_reps_numbers=? WHERE id=?",
                                           i.id, i.max_reps_numbers_id, i.max_reps_numbers, i.id) == 0)
                            sqlcon.Insert(i, typeof(Max_Reps_Numbers));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Max_Reps_Numbers - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateMinutes(List<Minutes> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Minutes - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateMinutes(Minutes i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Minutes SET id=?, " +
                            "minutes_id=?,minutes_for_emom=?,minutes_for_roulette=?,seconds_for_roulette=? WHERE id=?",
                                           i.id, i.minutes_id, i.minutes_for_emom,
                                           i.minutes_for_roulette, i.seconds_for_roulette, i.id) == 0)
                            sqlcon.Insert(i, typeof(Minutes));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Minutes - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateMovements(List<Movements> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Movements - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateMovements(Movements i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Movements SET id=?, " +
                            "movement_id=?,movement_name=? WHERE id=?",
                                           i.id, i.movement_id, i.movement_name, i.id) == 0)
                            sqlcon.Insert(i, typeof(Movements));
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Movements - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateOlyLifts(List<Oly_Lifts> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Oly_Lifts - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateOlyLifts(Oly_Lifts i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Oly_Lifts SET id=?, " +
                            "oly_lifts_id=?,oly_lifts_snatch_weight=?,oly_lifts_clean_and_jerk_weight=?,oly_lifts_date=? WHERE id=?",
                                           i.id, i.oly_lifts_id, i.oly_lifts_snatch_weight,
                                           i.oly_lifts_clean_and_jerk_weight, i.oly_lifts_date, i.id) == 0)
                            sqlcon.Insert(i, typeof(Oly_Lifts));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Oly_Lifts - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateRMMaxReps(List<RM_Max_Reps> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in RM_Max_Reps - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateRMMaxReps(RM_Max_Reps i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE RM_Max_Reps SET id=?, " +
                            "max_reps_id=?,max_reps=?,strength_oly_lifts=?,user_name=?," +
                            "max_weight=?, stength_lift_id=?, user_id=? WHERE id=?",
                                           i.id, i.max_reps_id, i.max_reps,
                                           i.strength_oly_lifts, i.user_name,
                                           i.max_weight, i.stength_lift_id, i.user_id, i.id) == 0)
                            sqlcon.Insert(i, typeof(RM_Max_Reps));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in RM_Max_Reps - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateRXScaled(List<RXScaled> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in RXScaled - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateRXScaled(RXScaled i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE RXScaled SET id=?, " +
                            "rxscaled_id=?,rxscaled_name=? WHERE id=?",
                                           i.id, i.rxscaled_id, i.rxscaled_name, i.id) == 0)
                            sqlcon.Insert(i, typeof(RXScaled));
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in RXScaled - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateAccessoryCategories(List<Accessory_Categories> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Accessory_Categories - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateAccessoryCategories(Accessory_Categories i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Accessory_Categories SET id=?, " +
                            "accessory_category_id=?,accessory_category_name=? WHERE id=?",
                                           i.id, i.accessory_category_id, i.accessory_category_name, i.id) == 0)
                            sqlcon.Insert(i, typeof(Accessory_Categories));
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Accessory_Categories - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateStrengthOlyLifts(List<Strength_Oly_Lifts> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Strength_Oly_Lifts - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateStrengthOlyLifts(Strength_Oly_Lifts i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Strength_Oly_Lifts SET id=?, " +
                            "strength_oly_id=?,strength_oly_lifts=?,strength_oly_lifts_lift=? WHERE id=?",
                                           i.id, i.strength_oly_id, i.strength_oly_lifts,
                                           i.strength_oly_lifts_lift, i.id) == 0)
                            sqlcon.Insert(i, typeof(Strength_Oly_Lifts));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Strength_Oly_Lifts - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateWodByDay(List<Wod_By_Day> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Wod_By_Day - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateWodByDay(Wod_By_Day i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Wod_By_Day SET id=?, " +
                            "wod_by_day_id=?,wod_date=?,wod_name=?,wod=? WHERE id=?",
                                           i.id, i.wod_by_day_id, i.wod_date,
                                           i.wod_name, i.wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Wod_By_Day));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Wod_By_Day - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateWodNumber(List<Wod_Number> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Wod_Number - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateWodNumber(Wod_Number i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Wod_Number SET id=?, " +
                            "wod_number_id=?,wod_number_wod=? WHERE id=?",
                                           i.id, i.wod_number_id, i.wod_number_wod, i.id) == 0)
                            sqlcon.Insert(i, typeof(Wod_Number));
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Wod_Number - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateStrengthWodScores(List<Wod_Scores> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Wod_Scores - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateStrengthWodScores(Wod_Scores i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Wod_Scores SET id=?, " +
                            "wod_scores_id=?,wod_date=?,user_name=?, rx_or_scaled=?, " +
                            "weight=?, time=?, reps=?, user_id=?, comments=? WHERE id=?",
                                           i.id, i.wod_scores_id, i.wod_date,
                                           i.user_name, i.rx_or_scaled,
                                           i.weight, i.time, i.reps, i.user_id, i.comments, i.id) == 0)
                            sqlcon.Insert(i, typeof(Wod_Scores));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in Wod_Scores - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateWSMFinals(List<WSM_Finals> ws)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        sqlcon.InsertOrReplaceAll(ws);
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in WSM_Finals - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateWSMFinals(WSM_Finals i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE WSM_Finals SET id=?, " +
                            "wsm_id=?,wsm_final_event=?,wsm_movements=? WHERE id=?",
                                           i.id, i.wsm_id, i.wsm_final_event,
                                           i.wsm_movements, i.id) == 0)
                            sqlcon.Insert(i, typeof(WSM_Finals));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in WSM_Finals - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }
        #endregion

        #region Getters

        #region PrivateNameConv

        private string GetName(string name)
        {
            var list = name.Split('.').ToList();
            if (list.Count == 1)
                return list[0];
            var last = list[list.Count - 1];
            return last;
        }

        #endregion

        public List<T> GetListOfObjects<T>(string id) where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("SELECT * FROM {0} WHERE id=?", GetName(typeof(T).ToString()));
                    var data = sqlcon.Query<T>(sql, id);
                    return data;
                }
            }
        }

        public List<T> GetListOfObjects<T>(string para, string val) where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("SELECT * FROM {0} WHERE {1}=?", GetName(typeof(T).ToString()), para);
                    var data = sqlcon.Query<T>(sql, val).ToList();
                    return data;
                }
            }
        }

        public List<T> GetListOfObjects<T>(string para1, string val1, string para2, string val2, bool ne = false) where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    string sign = ne ? "!=" : "=";
                    var sql = string.Format("SELECT * FROM {0} WHERE {1}{2}? AND {3}{4}?", GetName(typeof(T).ToString()), para1, sign, para2, sign);
                    var data = sqlcon.Query<T>(sql, val1, val2).ToList();
                    return data;
                }
            }
        }

        public List<T> GetListOfObjects<T>() where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("SELECT * FROM {0}", GetName(typeof(T).ToString()));
                    var data = sqlcon.Query<T>(sql, "").ToList();
                    return data;
                }
            }
        }

        public T GetSingleObject<T>(string id) where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("SELECT * FROM {0} WHERE id=?", GetName(typeof(T).ToString()));
                    var data = sqlcon.Query<T>(sql, id).FirstOrDefault(/*t => !t.is_deleted*/);
                    return data;
                }
            }
        }

        public T GetSingleObject<T>() where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("SELECT * FROM {0}", GetName(typeof(T).ToString()));
                    var data = sqlcon.Query<T>(sql, "").FirstOrDefault(/*t => !t.is_deleted*/);
                    return data;
                }
            }
        }

        public T GetSingleObject<T>(string para, string val) where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("SELECT * FROM {0} WHERE {1}=?", GetName(typeof(T).ToString()), para);
                    var data = sqlcon.Query<T>(sql, val).FirstOrDefault(/*t => !t.is_deleted*/);
                    return data;
                }
            }
        }

        public T GetSingleObject<T>(string para1, string val1, string para2, string val2, bool ne = false) where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    string sign = ne ? "!=" : "=";
                    var sql = string.Format("SELECT * FROM {0} WHERE {1}{2}? AND {3}{4}?", GetName(typeof(T).ToString()), para1, sign, para2, sign);
                    var data = sqlcon.Query<T>(sql, val1, val2).FirstOrDefault(/*t => !t.is_deleted*/);
                    return data;
                }
            }
        }

        public T GetSingleObject<T>(string para1, string val1, string para2, double val2) where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("SELECT * FROM {0} WHERE {1}=? AND {2}=?", GetName(typeof(T).ToString()), para1, para2);
                    var data = sqlcon.Query<T>(sql, val1, val2).FirstOrDefault(/*t => !t.is_deleted*/);
                    return data;
                }
            }
        }

        public List<T> GetObjectForUpdate<T>() where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("SELECT * FROM {0}", GetName(typeof(T).ToString()));
                    var data = sqlcon.Query<T>(sql, "");
                    return data;
                }
            }
        }

        #endregion

    }
}

