using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class Database : MonoBehaviour
{
    private int id = 1;
    private string nimi1 = "Matti";
    private string nimi2 = "Lotta";

    private string tableName = "Pelaaja";


    private string databaseName = "Userdata.db";

    // Start is called before the first frame update
    void Start()
    {

        createDataBase(databaseName);
        createTable(tableName);

        printData();
        addData(id, nimi1);
        id++;
        addData(id, nimi2);
        id++;
        printData();

        removeData(1);
        printData();
        addData(id, nimi1);
        id++;
        printData();
        addData(id, nimi2);
        id++;
        printData();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void createDataBase(string dbName)
    {
        string connection = "URI=file:" + Application.dataPath + "/" + dbName + "";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        dbcon.Close();
        dbcon = null;
    }
    void createTable(string tName)
    {
        IDbCommand dbcmd;
        IDataReader reader;

        string connection = "URI=file:" + Application.dataPath + "/" + databaseName + "";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS " + tName + "(id INTEGER PRIMARY KEY, nimi TEXT )";
        dbcmd.CommandText = q_createTable;
        reader = dbcmd.ExecuteReader();

        dbcon.Close();
        dbcon = null;
    }
    void printData()
    {
        string conn = "URI=file:" + Application.dataPath + "/" + databaseName + "";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT id, nimi " + "FROM " + tableName + "Pelaaja";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string nickname = reader.GetString(1);

            Debug.Log("Id: " + id + "  Nimi: " + nimi1);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    void addData(int id, string nimi)
    {
        string conn = "URI=file:" + Application.dataPath + "/" + databaseName + "";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO " + tableName + " (id, nimi) VALUES ('" + id + "', '" + nimi + "')";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    void removeData(int id)
    {
        string conn = "URI=file:" + Application.dataPath + "/" + databaseName + "";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM " + tableName + " WHERE id = '" + id + "'";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;
    }
}