﻿
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
using Android.Support.V7.App;

using Android.Views.Animations;
using Android.Animation;
using Clans.Fab;

namespace FAB.Demo
{
    [Activity(Label = "@string/floating_action_menu")]			
    public class FloatingMenusActivity : AppCompatActivity
    {
        private FloatingActionButton fab1;
        private FloatingActionButton fab2;
        private FloatingActionButton fab3;

        private FloatingActionButton fab12;
        private FloatingActionButton fab22;
        private FloatingActionButton fab32;

        private List<FloatingActionMenu> menus = new List<FloatingActionMenu>(6);
        private Handler mUiHandler = new Handler();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.floating_menus_activity);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            FloatingActionMenu menu1 = FindViewById<FloatingActionMenu>(Resource.Id.menu1);
            FloatingActionMenu menu2 = FindViewById<FloatingActionMenu>(Resource.Id.menu2);
            FloatingActionMenu menu3 = FindViewById<FloatingActionMenu>(Resource.Id.menu3);
            FloatingActionMenu menu4 = FindViewById<FloatingActionMenu>(Resource.Id.menu4);
            FloatingActionMenu menuDown = FindViewById<FloatingActionMenu>(Resource.Id.menu_down);
            FloatingActionMenu menuLabelsRight = FindViewById<FloatingActionMenu>(Resource.Id.menu_labels_right);

            FloatingActionButton programFab1 = new FloatingActionButton(this);

            programFab1.ButtonSize = FloatingActionButton.SizeMini;
            programFab1.LabelText = "Programmatically added button";
            programFab1.SetImageResource(Resource.Drawable.ic_edit);
            menu1.AddMenuButton(programFab1);

            ContextThemeWrapper context = new ContextThemeWrapper(this, Resource.Style.MenuButtonsStyle);
            FloatingActionButton programFab2 = new FloatingActionButton(context);
            programFab2.LabelText = "Programmatically added button";
            programFab2.SetImageResource(Resource.Drawable.ic_edit);
            menu2.AddMenuButton(programFab2);

            menus.Add(menuDown);
            menus.Add(menu1);
            menus.Add(menu2);
            menus.Add(menu3);
            menus.Add(menu4);
            menus.Add(menuLabelsRight);

            menuDown.HideMenuButton(false);
            menu1.HideMenuButton(false);
            menu2.HideMenuButton(false);
            menu3.HideMenuButton(false);
            menu4.HideMenuButton(false);
            menuLabelsRight.HideMenuButton(false);


            int delay = 400;
            foreach (var menu in menus)
            {
                mUiHandler.PostDelayed(() => menu.ShowMenuButton(true), delay);
                delay += 150;
            }

            menu1.SetClosedOnTouchOutside(true);

            menu4.IconAnimated = false;

            menu2.MenuToggle += (object sender, FloatingActionMenu.MenuToggleEventArgs e) => 
                {
                    String text = (e.Opened ? "Menu opened":"Menu closed");
                    Toast.MakeText(this, text, ToastLength.Short).Show();
                };

            fab1 = FindViewById<FloatingActionButton>(Resource.Id.fab1);
            fab2 = FindViewById<FloatingActionButton>(Resource.Id.fab2);
            fab3 = FindViewById<FloatingActionButton>(Resource.Id.fab3);

            fab12 = FindViewById<FloatingActionButton>(Resource.Id.fab12);
            fab22 = FindViewById<FloatingActionButton>(Resource.Id.fab22);
            fab32 = FindViewById<FloatingActionButton>(Resource.Id.fab32);

            fab1.Click += ActionButtonClickHandler;
            fab2.Click += ActionButtonClickHandler;
            fab3.Click += ActionButtonClickHandler;

            fab12.Click += ActionButtonClickHandler;
            fab22.Click += ActionButtonClickHandler;
            fab32.Click += ActionButtonClickHandler;


            FloatingActionButton fabEdit = FindViewById<FloatingActionButton>(Resource.Id.fab_edit);
            fabEdit.SetShowAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.scale_up));
            fabEdit.SetHideAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.scale_down));

            new Handler().PostDelayed(() => fabEdit.Show(true), delay + 150);

            fabEdit.Click += EditButtonClickHandler;

            CreateCustomAnimation();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void CreateCustomAnimation()
        {
            FloatingActionMenu menu3 = FindViewById<FloatingActionMenu>(Resource.Id.menu3);

            AnimatorSet set = new AnimatorSet();

            ObjectAnimator scaleOutX = ObjectAnimator.OfFloat(menu3.MenuIconView, "scaleX", 1.0f, 0.2f);
            ObjectAnimator scaleOutY = ObjectAnimator.OfFloat(menu3.MenuIconView, "scaleY", 1.0f, 0.2f);

            ObjectAnimator scaleInX = ObjectAnimator.OfFloat(menu3.MenuIconView, "scaleX", 0.2f, 1.0f);
            ObjectAnimator scaleInY = ObjectAnimator.OfFloat(menu3.MenuIconView, "scaleY", 0.2f, 1.0f);

            scaleOutX.SetDuration(50);
            scaleOutY.SetDuration(50);

            scaleInX.SetDuration(150);
            scaleInY.SetDuration(150);

            scaleInX.AnimationStart += (object sender, EventArgs e) =>
            {
                menu3.MenuIconView.SetImageResource(menu3.IsOpened ? Resource.Drawable.ic_close : Resource.Drawable.ic_star);
            };

            set.Play(scaleOutX).With(scaleOutY);
            set.Play(scaleInX).With(scaleInY).After(scaleOutX);
            set.SetInterpolator(new OvershootInterpolator(2));

            menu3.IconToggleAnimatorSet = set;
        }
    
        private void EditButtonClickHandler(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(RecyclerViewActivity)));
        }
    
        private void ActionButtonClickHandler(object sender, EventArgs e)
        {
            FloatingActionButton fabButton = sender as FloatingActionButton;
            if (fabButton != null)
            {
                Toast.MakeText(this, fabButton.LabelText, ToastLength.Short).Show();
            }
        }
    }
}



