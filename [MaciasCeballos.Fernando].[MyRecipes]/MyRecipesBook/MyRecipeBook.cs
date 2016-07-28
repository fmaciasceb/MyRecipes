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
        // Structure of each node of the XML
        public class Item
        {

            public string Link;
            public string Tittle;
            public string Summary;
            public DateTimeOffset LastUpdate;
            public DateTimeOffset PublishDate;

            public Item(Collection<SyndicationLink> lks, string tittle, string sum, DateTimeOffset dateUpdate, DateTimeOffset pubdate)
            {

                if (lks.Count > 0)
                {
                    this.Link = lks[0].Uri.AbsoluteUri.ToString();
                }
                this.Tittle = tittle;
                this.Summary = sum;
                this.LastUpdate = dateUpdate;
                this.PublishDate = pubdate;
            }


        }

        // Public function that returns nodes in an ArrayList

        public ArrayList GiveMeArrayList(string url, bool ShowErrors = false)
        {
            ArrayList Array = null;
            string errors = "";
            XmlReader reader;


            if (url.Trim() == "")
                return null;
            else
            {
                try
                {
                    reader = XmlReader.Create(url);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    Array = new ArrayList();
                    foreach (SyndicationItem item in feed.Items)
                    {
                        Item it = new Item(item.Links, item.Title.Text, item.Summary.Text, item.LastUpdatedTime, item.PublishDate);
                        Array.Add(it);
                    }

                    return Array;

                }
                catch
                {
                    errors += "Not valid URL";
                }
                if ((ShowErrors) && (errors != ""))
                    MessageBox.Show(errors);

                return Array;

            }
        }




    }





}
