// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Covi.Client.Services.Platform.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class NewsfeedArticles
    {
        /// <summary>
        /// Initializes a new instance of the NewsfeedArticles class.
        /// </summary>
        public NewsfeedArticles()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NewsfeedArticles class.
        /// </summary>
        public NewsfeedArticles(IList<ShortArticle> data = default(IList<ShortArticle>))
        {
            Data = data;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public IList<ShortArticle> Data { get; set; }

    }
}
