/*
 * File:        DAL.cs
 * Project:     DataTransferrer
 * Programmer:  David Pitters
 * Date:        March 12, 2017
 * Description: This file contains the Data Abstraction Layer class, which has all 
 *              connections and commands that are used in the transferring process.
 *              Re-used from Assignment 3.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace DataVisualizer
{
    public class DAL
    {
        public ADODB.Connection adodbCon;
        public SqlConnection con = new SqlConnection();

        public SqlCommand cmd = new SqlCommand();

        public SqlDataReader reader;

        public DAL()
        {

        }
    }
}