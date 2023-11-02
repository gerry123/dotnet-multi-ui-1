using System;
using Eto.Forms;
using Eto.Drawing;
using Microsoft.Data.Sqlite;
using System.IO;

namespace eto1
{
    public partial class MainForm : Form
    {
        Button buCreateTables;
        Button buClose;
        TextArea taLog;

        public MainForm()
        {
            Title = ".net eto.forms gtk demo 1";

            MinimumSize = new Size(800, 600);

            // create controls
            buCreateTables = new Button { Text = "Create tables" };

            buClose = new Button { Text = "Close" };

            taLog = new TextArea();
            //tbLog.AcceptsReturn = true;
            taLog.Height = 400;

            var tableLayoutMain = new TableLayout(
                        new TableRow(buCreateTables),
                        new TableRow(taLog),
                        new TableRow(buClose),
                        null // same as creating a row with ScaleHeight = true
                    )
            {
                Spacing = new Size(5, 5),
                Padding = new Padding(10)
            };

            // Add events
            buCreateTables.Click += (s, e) =>
            {
                //MessageBox.Show(this, "I was clicked!");
                Log("Trying to open database connection");
                if (OpenDb())
                {
                    Log("Database connection is opened");
                }
                else
                {
                    Log("Database connection is not valid");
                }
            };

            buClose.Click += (s, e) =>
            {
                this.Close();
            };

            // set content
            Content = tableLayoutMain;

            // create a few commands that can be used for the menu and toolbar
            var clickMe = new Command { MenuText = "Click Me!", ToolBarText = "Click Me!" };
            clickMe.Executed += (sender, e) => MessageBox.Show(this, "I was clicked!");

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) =>
            {
                MessageBox.Show(this, string.Format("I am:\n {0}", System.Reflection.Assembly.GetExecutingAssembly().CodeBase));
                //var ad = new AboutDialog(System.Reflection.Assembly.GetExecutingAssembly());
                //ad.ShowDialog(Application.Instance.MainForm);
                //.ShowDialog(this);
            };


            // create menu
            Menu = new MenuBar
            {
                Items =
                {
					// File submenu
					new SubMenuItem { Text = "&File", Items = { clickMe } },
                },
                ApplicationItems =
                {
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };

            // create toolbar			
            //ToolBar = new ToolBar { Items = { clickMe } };

        }

        void Log(string s)
        {
            taLog.Text = taLog.Text + s + Environment.NewLine;
        }

        bool OpenDb()
        {
            bool result = false;
            string dbFullPath = "";
            try
            {
                dbFullPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                 "eto1.db");
                var connectionStringBuilder = new SqliteConnectionStringBuilder
                {
                    Mode = SqliteOpenMode.ReadWriteCreate,
                    DataSource = dbFullPath
                };
                var connectionString = connectionStringBuilder.ToString();
                SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                CreateTables(connection);


                result = true;
            }
            catch (Exception E)
            {
                Log("Error:" + E.Message + Environment.NewLine + dbFullPath);
            }
            return result;
        }

        bool CreateTables(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText =
            @"
CREATE TABLE IF NOT EXISTS Person_Ps (
	IdPs INT PRIMARY KEY,
	FirstNamePs Text,
	LastNamePs Text
)
    ";
            command.ExecuteNonQuery();
            return true;
        }


    }
}
