using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.Net;


//*******************************************************************************************************
//******** My Recipe Book *******************************************************************************
//******** A simple library that receives an URL containing RSS feeds and returns the parsed information
//******** Author: Fernando Macías Ceballos *************************************************************
//******** Date: 2016-07-28 *****************************************************************************
//*******************************************************************************************************

namespace RecipeBook
{
    public class RecipeBook
    {
        private const int TimeOut = 20000; // limit 20s of loading

        // Structure of each node of the XML
        public class Item
        {
            public string ID;
            public string Link;
            public string Tittle;
            public string Summary;
            public DateTimeOffset LastUpdate;
            public DateTimeOffset PublishDate;
            public bool Unread;



            //Overload the equal operator to compare nodes

            public override bool Equals(object o)
            {
                if (o == null || o.GetType() != typeof(Item))
                    return false;
                Item ca = (Item)o;
                return ca.ID.Equals(this.ID);
            }

            public override int GetHashCode()
            {
                return ID.GetHashCode();
            }

            public Item(string id, Collection<SyndicationLink> lks, string tittle, string sum, DateTimeOffset dateUpdate, DateTimeOffset pubdate, bool unread)
            {
                this.ID = id;
                if (lks.Count > 0)
                {
                    this.Link = lks[0].Uri.AbsoluteUri.ToString();
                }
                this.Tittle = tittle;
                this.Summary = sum;
                this.LastUpdate = dateUpdate;
                this.PublishDate = pubdate;
                this.Unread = unread;
            }


        }

        // Public function that returns nodes in an ArrayList

        public ArrayList GiveMeArrayList(string url, bool ShowErrors = false)
        {
            ArrayList Array = null;
            string errors = "";
          
            if (url.Trim() == "")
                return null;
            else
            {
                try
                {


                    WebRequest request = WebRequest.Create(url);
                    request.Timeout = TimeOut;

                    using (WebResponse response = request.GetResponse())
                    using (XmlReader lct = XmlReader.Create(response.GetResponseStream()))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(lct);
                        lct.Close();
                        Array = new ArrayList();
                        foreach (SyndicationItem item in feed.Items)
                        {
                            Item it = new Item(item.Id, item.Links, item.Title.Text, item.Summary.Text, item.LastUpdatedTime, item.PublishDate, true);
                            Array.Add(it);
                        }
                    }
                    return Array;

                }
                catch
                {
                    errors += "Not valid URL or timeout exceeded";
                }
                if ((ShowErrors) && (errors != ""))
                    MessageBox.Show(errors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return Array;

            }
        }




    }





}
