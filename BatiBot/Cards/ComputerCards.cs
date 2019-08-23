using BatiBot.Models;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatiBot.Cards
{
    public class ComputerCards
    {
        public static HeroCard GetAsusCard()
        {
            
            var asusCard = new HeroCard
            {
                Title = "Asus",
                Subtitle = "Gaming Laptop",
                Text = "Boost up your gaming peripherals with the new Asus gaming laptop",
                Images = new List<CardImage> { new CardImage("https://www.canex.ca/media/catalog/product/cache/1/image/9df78eab33525d08d6e5fb8d27136e95/2/0/201804am060000007_15229554156691450037759.jpg") },
                Buttons = new List<CardAction> { new CardAction (ActionTypes.PostBack, "Pick me!", value:"Asus") }
            };
            return asusCard;
            
        }
        public static HeroCard GetAcerCard()
        {
            var acerCard = new HeroCard
            {
                Title = "Acer",
                Subtitle = "Gaming Laptop",
                Text = "Boost up your gaming peripherals with the new Acer gaming laptop",
                Images = new List<CardImage> { new CardImage("https://brain-images-ssl.cdn.dixons.com/4/4/10180744/l_10180744_002.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, value: "Acer", title: "Pick Me!") }
            };
            return acerCard;
        }
        public static HeroCard GetLenovoCard()
        {
            var lenovoCard = new HeroCard
            {
                Title = "Lenovo",
                Subtitle = "Gaming Laptop",
                Text = "Boost up your gaming peripherals with the new Acer gaming laptop",
                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-2.amazonaws.com/wc-prod-pim/JPEG_1000x1000/SLIPS130CL_E_lenovo_ideapad_s130_14_celeron_laptop.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, value: "Lenovo", title: "Pick Me!") }
            };
            return lenovoCard;
        }
    }
}
