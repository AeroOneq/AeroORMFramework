using Newtonsoft.Json;

namespace AeroORMFramework.CustomTypes
{
    /// <summary>
    /// Stores the json STRING representation of the object
    /// </summary>
    /// <typeparam name="Type">Type can be any type</typeparam>
    public class Json<Type>
    {
        private string JsonString { get; } 

        public Json(Type jsonObject)
        {
            JsonString = JsonConvert.SerializeObject(jsonObject);
        }
    }
}
