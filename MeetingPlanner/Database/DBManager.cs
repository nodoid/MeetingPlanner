using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SQLite.Net;

namespace MeetingPlanner
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
                        sqlcon.CreateTable<Attendees>();
                        sqlcon.CreateTable<AppointmentList>();
                        sqlcon.CreateTable<Meeting>();
                        sqlcon.CreateTable<Polling>();
                        sqlcon.CreateTable<PollingData>();
                        sqlcon.CreateTable<PollCast>();
                        sqlcon.CreateTable<Times>();
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
                        sqlcon.Execute("DELETE FROM AppointmentList");
                        sqlcon.Execute("DELETE FROM Attendees");
                        sqlcon.Execute("DELETE FROM Meeting");
                        sqlcon.Execute("DELETE FROM Polling");
                        sqlcon.Execute("DELETE FROM PollingData");
                        sqlcon.Execute("DELETE FROM PollCast");
                        sqlcon.Execute("DELETE FROM Times");
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

        public void DeleteObject<T>(int imp) where T : IInterface
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        var command = string.Format("DELETE FROM {0} WHERE id={1}", typeof(T).ToString(), imp);
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

        public void AddOrUpdateAppointments(List<AppointmentList> ac)
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
                        Debug.WriteLine("Error in AddOrUpdateAppointments - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateAppointments(AppointmentList ac)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE AppointmentList SET " +
                            "MeetingName=?, DateTimeFrom=?,Length=?, Room=?, Venue=?, MeetingId=?, DateCreated=?, DateDue=?, UserId=? WHERE id=?",
                                           ac.MeetingName, ac.DateTimeFrom, ac.Length, ac.Room, ac.Venue, ac.MeetingId, ac.DateCreated, ac.DateDue, ac.UserId, ac.id) == 0)
                            sqlcon.Insert(ac, typeof(AppointmentList));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateAppointments - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateAttendees(List<Attendees> user)
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
                        Debug.WriteLine("Error in AddOrUpdateAttendees - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateAttendees(Attendees i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Attendees SET MeetingId=?,UserId=?," +
                            "Name=?,Email=?,Attending=?,IsOrganiser=? WHERE id=?",
                                           i.MeetingId, i.UserId, i.Name, i.Email, i.Attending, i.IsOrganiser, i.id) == 0)
                            sqlcon.Insert(i, typeof(Attendees));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateAttendees - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdatePolling(List<Polling> user)
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
                        Debug.WriteLine("Error in AddOrUpdatePolling - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdatePolling(Polling i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Polling SET MeetingId=?," +
                            "PollId=? WHERE id=?",
                                           i.MeetingId, i.PollId, i.id) == 0)
                            sqlcon.Insert(i, typeof(Polling));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdatePolling - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdatePollingData(List<PollingData> user)
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
                        Debug.WriteLine("Error in AddOrUpdatePollingData - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdatePollingData(PollingData i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE PollingData SET TotalVotesReceived=?," +
                            "Votes=?, PollId=? WHERE id=?",
                                           i.TotalVotesReceived, i.Votes, i.PollId, i.id) == 0)
                            sqlcon.Insert(i, typeof(PollingData));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdatePollingData - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateMeeting(List<Meeting> user)
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
                        Debug.WriteLine("Error in AddOrUpdateMeeting - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateMeeting(Meeting i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Meeting SET MeetingSchedule=?," +
                            "IsMyMeeting=?, IveResponded=? WHERE id=?",
                                           i.MeetingSchedule, i.IsMyMeeting, i.IveResponded, i.id) == 0)
                            sqlcon.Insert(i, typeof(Meeting));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateMeeting - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdatePollCast(List<PollCast> user)
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
                        Debug.WriteLine("Error in AddOrUpdatePollCast - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdatePollCast(PollCast i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE PollCast SET PollDataId=?," +
                            "Accept=?, AttendeeId=? WHERE id=?",
                                           i.PollDataId, i.Accept, i.AttendeeId, i.id) == 0)
                            sqlcon.Insert(i, typeof(PollCast));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdatePollCast - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateTimes(List<Times> user)
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
                        Debug.WriteLine("Error in AddOrUpdateTimes - {0}--{1}", ex.Message, ex.StackTrace);
#endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        public void AddOrUpdateTimes(Times i)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Times SET MeetingId=?," +
                            "Time=? WHERE id=?",
                                           i.MeetingId, i.Time, i.id) == 0)
                            sqlcon.Insert(i, typeof(Times));
                        sqlcon.Commit();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine("Error in AddOrUpdateTimes - {0}--{1}", ex.Message, ex.StackTrace);
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

        public List<T> GetListOfObjects<T, U>(string para, U val) where T : class, IInterface, new()
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

        public T GetSingleObject<T, U>(string para1, U val1, bool ne = false) where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    string sign = ne ? "!=" : "=";
                    var sql = string.Format("SELECT * FROM {0} WHERE {1}{2}?", GetName(typeof(T).ToString()), para1, sign);
                    var data = sqlcon.Query<T>(sql, val1).FirstOrDefault();
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

        public int GetLastId<T>() where T : class, IInterface, new()
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(App.Self.SQLitePlatform, App.Self.ConnectionString))
                {
                    sqlcon.Execute(DBClauseSyncOff);
                    var sql = string.Format("select id from {0}", GetName(typeof(T).ToString()));
                    var data = sqlcon.Query<T>(sql, "").ToList().Count;
                    return data + 1;
                }
            }
        }

        #endregion

    }
}

