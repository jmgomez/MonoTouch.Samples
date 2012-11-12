using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

using Vici.CoolStorage;
namespace ViciCoolStorage
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{

		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			

			ComprobarYCrearLaDB();
			//Se crea la interfaz de usuario
	
			var root = new RootElement("Personas");
			var section = new Section("Personas");
			foreach (var p in Persona.List()) {
				section.Add(new StringElement(string.Format("{0} {1}",p.Nombre,p.Apellidos)));
			}
			root.Add(section);
			var dialog = new DialogViewController(UITableViewStyle.Plain,root);
			dialog.Root=root;


			window.RootViewController = dialog;
			window.MakeKeyAndVisible ();
			
			return true;
		}


		
		protected void ComprobarYCrearLaDB ()
		{	
			string dbNombre = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "db00001");


			CSConfig.SetDB (dbNombre,SqliteOption.CreateIfNotExists, () => {
				CSDatabase.ExecuteNonQuery (
					"CREATE TABLE Persona (Id INTEGER PRIMARY KEY AUTOINCREMENT," +
					"Nombre text, Apellidos text)");
			
					//Datos de ejemplo
					Persona p = new Persona(){ Nombre="Jose", Apellidos="Gónzalez"};
					p.Save();
					p = new Persona(){ Nombre="Antonio", Apellidos="Gónzalez"};
					p.Save();


			});

	}



	[MapTo("Persona")]
	public class Persona : CSObject<Persona,int>{
		public int Id {
			get { return (int)GetField("Id"); }
		}
		public string Nombre {
			get { return (string)GetField("Nombre"); }
			set { SetField("Nombre",value); }
		}
		public string Apellidos {
			get { return (string)GetField("Apellidos"); }
			set { SetField("Apellidos",value); }
		}
		
	}



}
}

