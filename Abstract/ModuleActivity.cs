using SupportActivity = Android.Support.V7.App.AppCompatActivity;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportDrawerLayout = Android.Support.V4.Widget.DrawerLayout;

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

using Supermortal.Common.Droid.Concrete;
using Supermortal.Common.Droid.Enums;

namespace Supermortal.Common.Droid.Abstract
{
    [Activity(Label = "ModuleActivity")]			
    public abstract class ModuleActivity : SupportActivity
    {

        private int _nextCountId = 0;

        private Dictionary<int, Type> ModuleTypes = new Dictionary<int, Type>();
        private Dictionary<Type, int> ModuleTypeToIdMap = new Dictionary<Type, int>();
        protected Dictionary<int, ModuleFragment> Modules = new Dictionary<int, ModuleFragment>();
        protected Dictionary<int, string> ModuleTitles = new Dictionary<int, string>();

        #region Drawer Properties

        private string[] _drawerTitles;

        protected string[] DrawerTitles
        { 
            get
            { 
                return _drawerTitles ?? (_drawerTitles = ModuleTitles.Values.ToArray());
            } 
        }

        protected DrawerState DrawerState { get; private set; }

        protected ArrayAdapter _drawerAdapter;
        protected int _drawerViewResourceId;
        protected int _drawerLayoutResourceId;
        protected ModuleDrawerToggle _drawerToggle;
        protected int _drawerItemResourceId;
        protected int _drawerOpenResourceId;
        protected int _drawerClosedResourceId;
        protected SupportDrawerLayout _drawerLayout;
        protected ListView _drawerView;
        protected int _mainContentResourceId;

        #endregion

        #region Drawer

        protected void SetupDrawer(int drawerItemResourceId, int drawerViewResourceId, int drawerLayoutResourceId, int drawerOpenResourceId, int drawerClosedResourceId, int mainContentResourceId)
        {
            _drawerViewResourceId = drawerViewResourceId;
            _drawerLayoutResourceId = drawerLayoutResourceId;
            _drawerItemResourceId = drawerItemResourceId;
            _drawerOpenResourceId = drawerOpenResourceId;
            _drawerClosedResourceId = drawerClosedResourceId;
            _mainContentResourceId = mainContentResourceId;

            SetupDrawer();
        }

        private void SetupDrawer()
        {
            if (_drawerLayout != null && _drawerView != null)
                CloseDrawer();

            _drawerLayout = FindViewById<SupportDrawerLayout>(_drawerLayoutResourceId);
            _drawerView = FindViewById<ListView>(_drawerViewResourceId);

            _drawerAdapter = new ArrayAdapter<string>(this, _drawerItemResourceId, DrawerTitles);
            _drawerView.Adapter = _drawerAdapter;
            _drawerView.ItemClick += Drawer_ItemClick;

            _drawerToggle = new ModuleDrawerToggle(
                this,                           //Host Activity
                _drawerLayout,                  //DrawerLayout
                _drawerOpenResourceId,     //Opened Message
                _drawerClosedResourceId     //Closed Message
            );

            _drawerLayout.SetDrawerListener(_drawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            _drawerToggle.SyncState();

            DrawerState = DrawerState.Closed;
        }

        protected void Drawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ShowFragment((int)e.Id);
            CloseDrawer();
        }

        protected void ShowFragment(int itemId)
        {
            var module = GetFragmentInstance(itemId);
            ShowFragment(module);
        }

        protected void ShowFragment(ModuleFragment module)
        {
            //TODO deal with authentication

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Replace(_mainContentResourceId, module);
            trans.Commit();
        }

        #endregion

        #region Lifecycle

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _drawerToggle.OnConfigurationChanged(newConfig);
        }

        #endregion

        #region Methods

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
            SetupDrawer();
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

        protected void OpenDrawer()
        {
            _drawerLayout.OpenDrawer(_drawerView);
            DrawerState = DrawerState.Open;
            _drawerToggle.SyncState();
        }

        protected void ToggleDrawer()
        {
            if (_drawerLayout.IsDrawerOpen(_drawerView))
            {
                CloseDrawer();
            }
            else
            {
                OpenDrawer();
            }
        }

        protected void CloseDrawer()
        {
            _drawerLayout.CloseDrawer(_drawerView);
            DrawerState = DrawerState.Closed;
            _drawerToggle.SyncState();
        }

        #endregion
    }
}

