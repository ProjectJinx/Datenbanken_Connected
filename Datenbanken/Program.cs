using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Threading;

namespace Datenbanken
{
    class Program
    {
        static void Main(string[] args)
        {

            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=./Nordwind.mdb");
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = conn;
            string s, desc;
            do
            {
                Console.WriteLine("Ändern(A) / Zeigen(Z) / Close(X)");
                desc = Console.ReadLine();



                switch (desc)
                {
                    case "z":
                    case "Z":
                        {
                            Console.WriteLine("Tabelle:");
                            s = Console.ReadLine();
                            while (s == "")
                            {
                                Console.WriteLine("Name darf nicht leer sein \n" +
                                    "Tabelle:");
                                s = Console.ReadLine();
                            }
                            try
                            {
                                cmd.CommandText = "SELECT * FROM " + s;
                                conn.Open();
                                OleDbDataReader read = cmd.ExecuteReader();
                                int col = read.FieldCount;
                                while (read.Read())
                                {
                                    for (int i = 0; i < col; i++)
                                    {
                                        Console.WriteLine(Convert.ToString(read.GetValue(i)) + " ");
                                    }
                                    Console.Write("\n");
                                    Thread.Sleep(5);
                                }
                            }
                            catch (OleDbException e)
                            {
                                Console.WriteLine("Tabelle nicht vorhanden");
                            }
                            catch (InvalidCastException)
                            {
                                //
                            }
                            finally
                            {
                                conn.Close();
                            }

                            Console.WriteLine("\n'X' drücken zum Beenden");
                        }
                        break;
                    case "a":
                    case "A":
                        string inp;
                        do
                        {
                            Console.WriteLine("Hinzufügen(H) / Löschen(L)?");
                            inp = Console.ReadLine();
                        }
                        while (inp == "");
                        switch (inp)
                        {
                            case "h":
                            case "H":
                                try
                                {
                                    Console.WriteLine("Firma:");
                                    string firma = Console.ReadLine();

                                    Console.WriteLine("Rufnummer:");
                                    string nummer = Console.ReadLine();

                                    cmd.CommandText = "INSERT INTO Versandfirmen (Firma, Telefon) VALUES (?, ?)";
                                    cmd.Parameters.Add("Firma", OleDbType.VarWChar, 50);
                                    cmd.Parameters.Add("Telefon", OleDbType.VarWChar, 20);
                                    cmd.Parameters[0].Value = firma;
                                    cmd.Parameters[1].Value = nummer;
                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Eintrag erfolgreich hinzugefügt."); 
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                                break;
                            case "l":
                            case "L":
                                try
                                {
                                    Console.WriteLine("Firma:");
                                    string firma = Console.ReadLine();
                                    cmd.CommandText = "DELETE FROM Versandfirmen WHERE Firma='" + firma + "'";
                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Eintrag erfolgreich gelöscht.");
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                                break;
                        }
                        break;
                    case "x":
                    case "X":
                        Environment.Exit(0);
                        break;

                }

            }
            while (true);
        }
    }
}
