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
using HackAtHome.SAL;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon_bol")]
    public class EvidenceDetailActivity : Activity
    {
        TextView txtInfo, txtTitle, txtStatus, txtDescribe;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EvidenceDetail);

            txtInfo = FindViewById<TextView>(Resource.Id.txtInfoUser);
            txtTitle = FindViewById<TextView>(Resource.Id.textViewTitle);
            txtStatus = FindViewById<TextView>(Resource.Id.textViewStatus);
            txtTitle = FindViewById<TextView>(Resource.Id.textViewTitle);
            txtDescribe = FindViewById<TextView>(Resource.Id.textViewDescribe);

            txtInfo.Text = Intent.GetStringExtra("name");
            getDetail(Intent.GetStringExtra("token"), Intent.GetStringExtra("id"));
        }
        async void getDetail(string token, string id)
        {
            var client = new ServiceCaller();
            var detail = await client.GetEvidenceByIDAsync(token, int.Parse(id));

            txtDescribe.Text = detail.Description;

        }
    }
}