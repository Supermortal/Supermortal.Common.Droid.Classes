using SupportFragment = Android.Support.V4.App.Fragment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Supermortal.Common.Droid.Classes.Abstract
{
    public abstract class ModuleFragment : SupportFragment
    {

        public abstract string ModuleTitle { get; set; }

        protected virtual void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        protected virtual View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}

