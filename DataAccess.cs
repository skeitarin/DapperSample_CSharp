using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

/*
* Dapperを利用するにはパッケージマネージャコンソールで
*     Install-Package Dapper
* と入力して、予めライブラリをプロジェクトに参照設定しておきます。
*/
using Dapper;

namespace KSB.BusinessLayer.Tests.Util
{

    public class DataAccess
    {
        /*
        * sqlでバインド変数を使用する場合は、@マークを使用します。
        *  ex) select name from syain where syain_code = @SYAIN_CODE
        * 第二引数にはsqlにバインドする値をセットします。
        * モデルクラスや匿名型を渡すことでDapperがsqlにバインドします。
        *  ex) var param = new {SYAIN_CODE = "1001"} <-匿名型
        */
        public int ExecuteNonQuery(string sql, dynamic param)
        {
            var execCnt = 0;
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = "DB接続文字列";
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // using Dapper
                        execCnt = conn.Execute(sql, (object)param, tran);
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                    }
                }
            }
            return execCnt;
        }

        public T QueryForModel<T>(string sql, dynamic param)
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = "DB接続文字列";
                conn.Open();
                try
                {
                    // using Dapper
                    return conn.Query<T>(sql, (object)param).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public IEnumerable<T> QueryForListModel<T>(string sql, dynamic param)
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = "DB接続文字列";
                conn.Open();
                try
                {
                    // using Dapper
                    return conn.Query<T>(sql, (object)param);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }    
    }
}
