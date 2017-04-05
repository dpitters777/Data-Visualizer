/*
 * File:        DataVisualizer.aspx.cs
 * Project:     DataVisualizer
 * Programmer:  David Pitters
 * Date:        March 26, 2017
 * Description: This file contains the code behind for the Data Visualizer.
 *              First, it connects to a database and loads the rows from the
 *              category and store tables into two dropdowns. Once the user
 *              chooses a category and store, the products available for those
 *              filters are loaded. When the user clicks the "refresh" button,
 *              the graph displaying the product's sales over time is refreshed
 *              with the new data.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Drawing;

namespace DataVisualizer
{
    public partial class _Default : Page
    {
        DAL dal;

        protected void Page_Load(object sender, EventArgs e)
        {
            noResultsMessage.Visible = false; 

            dal = new DAL();
            SetDatabase(); //Connect to the user's selected database

            //Load the categories and stores on first startup only
            if (!IsPostBack)
            {
                string categoryValueField = "product_class_id";
                string categoryTextField = "product_subcategory";
                string categoryTable = "product_class";
                LoadRows(categoryValueField, categoryTextField, categoryTable, null, CategoryDropdown);

                string storeValueField = "store_id";
                string storeTextField = "store_city";
                string storeTable = "store";
                LoadRows(storeValueField, storeTextField, storeTable, null, StoreDropdown);
            }
            
        }

        /*
         * METHOD:      LoadRows
         * PARAMETERS:  string valueField, string textField, string table, DropDownList dropdown, List<string> column
         * RETURNS:     Nothing
         * DETAIL:      This method loads rows from a given column into a dropdown list.
        */
        private void LoadRows(string valueField, string textField, string table, string where, DropDownList dropdown)
        {
            string query = "";
            //Put together the query with the given pieces
            if (where != null)
            {
                query = "SELECT " + valueField + ", " + textField + " FROM " + table + " " + where + ";";
            }
            else
            {
                query = "SELECT " + valueField + ", " + textField + " FROM " + table + ";";
            }
            dal.cmd.CommandText = query;
            dal.cmd.Connection = dal.con;
            try
            {
                //Bind the data from the given column to the corresponding dropdown
                dal.cmd.Connection.Open();
                dropdown.DataSource = dal.cmd.ExecuteReader();
                dropdown.DataTextField = textField;
                dropdown.DataValueField = valueField;
                dropdown.DataBind();
            }
            catch (Exception ex)
            {
                //Let the user know there was a problem doing the query
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('"+ex.Message+"')", true);
            }
            finally
            {
                dal.cmd.Connection.Close();
            }
        }

        /*
         * METHOD:      SetADatabase
         * PARAMETERS:  None
         * RETURNS:     Nothing
         * DETAIL:      This method sets the database using the web.config connection string.
        */
        private void SetDatabase()
        {
            dal.con.ConnectionString = ConfigurationManager.ConnectionStrings["foodmartConnection"].ConnectionString;
            
            //Test Connection to the source database
            try
            {
                dal.con.Open();
                dal.con.Close();
                //Let the user know they connected to the database successfully (only on first page load)
                if (!IsPostBack)
                {
                    string database = "Foodmart";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Connected to database "+database+" Successfully')", true);
                }
            }
            catch (Exception ex)
            {
                //Let the user know there was a problem connecting to the database
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Database connection failed')", true);
            }
        }

        /*
         * METHOD:      CategoryDropdown_SelectedIndexChanged
         * PARAMETERS:  object sender, EventArgs e
         * RETURNS:     Nothing
         * DETAIL:      This event handler loads all Products under the selected
         *              Product Category.
        */
        protected void CategoryDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the category changes, products available under that category will need up be updated as well.
            //If the category is "All", load all products
            //Clear the old items from the product dropdown.
            ProductDropdown.Items.Clear();

            ProductDropdown.Items.Add(new ListItem("--Select Product--"));

            ProductDropdown.AppendDataBoundItems = true;

            /*For the given product category, find all the products related to it.*/

            //Find the ID of the newly selected category
            string product_class_id = CategoryDropdown.SelectedValue; //This will return the value (ID) of the category
            string dataValueField = "";
            string dataTextField = "";
            string table = "";
            string where;
            if (CategoryDropdown.SelectedValue == "All")
            {
                //If the user chooses to filter by all categoryies, no WHERE clause is needed. Get every product.
                dataValueField = "product_id";
                dataTextField = "product_name";
                table = "product";
                where = null;
            }
            else
            {
                //The user selected a category. Get products only under that category.
                dataValueField = "product_id";
                dataTextField = "product_name";
                table = "product";
                where = "WHERE product_class_id =" + product_class_id;
            }
            LoadRows(dataValueField, dataTextField, table, where, ProductDropdown);

            //Products for the category have been loaded. Allow the user to now select a product.
            ProductDropdown.Enabled = true;
        }

        protected void ProductDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            //A product has been selected. Allow the user to now select a store.
            StoreDropdown.Enabled = true;
        }

        protected void refreshBtn_Click(object sender, EventArgs e)
        {
            //Clear the old data points
            dataChart.Series[0].Points.Clear();

            //Dipslay the sales for the selected product at the selected store
            string selectedProductID = ProductDropdown.SelectedValue;
            string selectedStoreID = StoreDropdown.SelectedValue;

            string query = "SELECT the_month, sum(store_sales) as total_sales FROM sales_fact_1998 INNER JOIN time_by_day ON time_by_day.time_id=sales_fact_1998.time_id INNER JOIN store ON store.store_id=sales_fact_1998.store_id WHERE product_id="+selectedProductID+" AND sales_fact_1998.store_id="+selectedStoreID+" GROUP BY the_month ORDER BY DATEPART(mm,CAST([the_month]+ ' 1900' AS DATETIME));";
            
            dal.cmd.CommandText = query;
            dal.cmd.Connection = dal.con;

            try
            {
                //Bind the data to the chart
                dal.cmd.Connection.Open();
                dataChart.DataSource = dal.cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataChart.Series[0].XValueMember = "the_month";
                dataChart.Series[0].YValueMembers = "total_sales";

                string prodName = ProductDropdown.SelectedItem.Text;
                string storeName = StoreDropdown.SelectedItem.Text;
                dataChart.Titles.Add("header");
                dataChart.Titles[0].Text = "Sales of " + prodName + " at " + storeName + " by Month";
                dataChart.ChartAreas[0].AxisX.Title = "Month";
                dataChart.ChartAreas[0].AxisY.Title = "Sales (in dollars)";

                dataChart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                dataChart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;

                dataChart.DataBind();
                if (dataChart.Series[0].Points.Count == 0)
                {
                    //No results for that search. Let the user know
                    noResultsMessage.Visible = true;
                    dataChart.Visible = false;
                }
                else
                {
                    //Show the data chart
                    dataChart.Visible = true;
                    noResultsMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Lookup failed. Please Try again.')", true);
            }
            finally
            {
                dal.cmd.Connection.Close();
            }
                
        }
    }
}