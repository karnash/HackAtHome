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
using HackAtHome.CustomAdapters;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon_bol")]
    public class EvidenceActivity : Activity
    {
        TextView txtInfo;
        ListView lstMaster;
        List<Evidence> evidencias;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Evidences);

            txtInfo = FindViewById<TextView>(Resource.Id.txtInfoUser);
            lstMaster = FindViewById<ListView>(Resource.Id.listViewEvidencias);
            lstMaster.ItemClick += LstMaster_ItemClick;
            txtInfo.Text = Intent.GetStringExtra("userName");
            getEvidencesList(Intent.GetStringExtra("token"));
        }

        private void LstMaster_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = evidencias.Where(x => x.EvidenceID == e.Id).FirstOrDefault();

            var detalle = new Intent(this, typeof(EvidenceDetailActivity));
            detalle.PutExtra("token", Intent.GetStringExtra("token"));
            detalle.PutExtra("id", item.EvidenceID);
            detalle.PutExtra("name", Intent.GetStringExtra("userName"));
            StartActivity(detalle);
        }

        async void getEvidencesList(string token)
        {
            var persiste = (Persistencia)this.FragmentManager.FindFragmentByTag("lista");
            if (persiste == null)
            {
                persiste = new Persistencia();
                var trans = this.FragmentManager.BeginTransaction();
                trans.Add(persiste, "lista");
                trans.Commit();

                var client = new ServiceCaller();
                evidencias = await client.GetEvidencesAsync(token);
                persiste.evidencias = evidencias;

            }
            else
            {
                evidencias = persiste.evidencias;
            }

            lstMaster.Adapter = new EvidencesAdapter(this, evidencias, Resource.Layout.listOfItems, Resource.Id.itemTitle, Resource.Id.itemStatus);
        }
    }
}