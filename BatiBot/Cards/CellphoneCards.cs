using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatiBot.Cards
{
    public class CellphoneCards
    {
        public static HeroCard SamsungCard()
        {
            var samsungCard = new HeroCard()
            {
                Title = "Samsung",
                Subtitle = "Samsung Series",
                Text = "Define best with Samsung!",
                Images = new List<CardImage> { new CardImage("https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/note10-aurablack-1565212490.jpg?crop=0.6665xw:1xh;center,top&resize=768:*") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, value: "Samsung", title: "Pick me!") }
            };
            return samsungCard;
        }
        public static HeroCard IphoneCard()
        {
            var iphoneCard = new HeroCard()
            {
                Title = "Apple",
                Subtitle = "Iphone Series",
                Text = "The future is here featuring Apple iPhones.",
                Images = new List <CardImage> { new CardImage("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone-xr-white-select-201809?wid=940&hei=1112&fmt=png-alpha&qlt=80&.v=1551226036668") },
                Buttons = new List <CardAction> { new CardAction(ActionTypes.PostBack, value:"Apple", title: "Pick me!") }
            };
            return iphoneCard;
        }

    }
}
