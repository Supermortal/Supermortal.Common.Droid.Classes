using SupportActivity = Android.Support.V7.App.AppCompatActivity;
using SupportFragment = Android.Support.V4.App.Fragment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Supermortal.Common.Droid.Classes.Abstract
{
    [Activity(Label = "ModuleActivity")]			
    public abstract class ModuleActivity : SupportActivity
    {

        private int _nextCountId = 0;

        private Dictionary<int, Type> ModuleTypes = new Dictionary<int, Type>();
        private Dictionary<Type, int> ModuleTypeToIdMap = new Dictionary<Type, int>();
        protected Dictionary<int, ModuleFragment> Modules = new Dictionary<int, ModuleFragment>();
        protected Dictionary<int, string> ModuleTitles = new Dictionary<int, string>();

        private string[] _drawerTitles;

        protected string[] DrawerTitles
        { 
            get
            { 
                return _drawerTitles ?? (_drawerTitles = ModuleTitles.Values.ToArray());
            } 
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected void RegisterModule<T>(string moduleTitle) where T : ModuleFragment
        {
            ModuleTypes.Add(_nextCountId, typeof(T));
            ModuleTypeToIdMap.Add(typeof(T), _nextCountId);
            ModuleTitles.Add(_nextCountId, moduleTitle);

            _nextCountId++;
        }

        protected void ResetDrawer()
        {
            _drawerTitles = null;
        }

        protected ModuleFragment GetFragmentInstance(int itemId)
        {
            if (Modules.ContainsKey(itemId))
                return Modules[itemId];

            if (!ModuleTypes.ContainsKey(itemId))
                return null;

            var module = (ModuleFragment)Activator.CreateInstance(ModuleTypes[itemId]);
            module.ModuleTitle = ModuleTitles[itemId];

            Modules.Add(itemId, module);

            return module;
        }

        protected T GetFragmentInstance<T>() where T : ModuleFragment
        {
            var moduleType = typeof(T);

            if (!ModuleTypeToIdMap.ContainsKey(moduleType))
                return default(T);

            var itemId = ModuleTypeToIdMap[moduleType];
            return (T)GetFragmentInstance(itemId);
        }
    }
}

