using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geodesy.Datum
{
    /// <summary>
    /// Identifier used to identify objects such as Datums, Ellipsoids or CoordinateReferenceSystems.
    /// Identifier encapsulates all identification info of Identifiable objects in a special instance to make object creation clearer.
    /// Identifier also offers new unique ids for every object created in the LOCAL namespace.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Identifier
    {
        #region static properties
        /// <summary>
        /// Namespace used to identify objects having no reference in an external persistent database.
        /// </summary>
        private const string Local = "LOCAL";

        /// <summary>
        /// Value used for objects with an unknown name.
        /// </summary>
        public const string Unknown = "UNKNOWN";


        /// <summary>
        /// Unique integer generated to identify a LOCAL object. LOCAL refers to a namespace
        /// </summary>
        private static int LocalID = 10000;
        #endregion

        /// <summary>
        /// Return an identifier which is unique for this program session. This identifier is 
        /// usually associated with the LOCAL namespace
        /// </summary>
        /// <returns>a new identifier</returns>
        public static string GetNewID()
        {
            return LocalID++.ToString();
        }

        #region constructor
        /// <summary>
        /// This private constructor is designed for JsonConvert.DeserializeObject().
        /// </summary>
        private Identifier()
        { }

        /// <summary>
        /// Creates a complete identifier.
        /// </summary>
        /// <param name="type">the class of the identified object</param>
        public Identifier(Type type)
            : this(Local + "_" + type.FullName, GetNewID(), Unknown, null, null, null)
        { }

        /// <summary>
        /// Creates a complete identifier.
        /// </summary>
        /// <param name="type">the class of the identified object</param>
        /// <param name="name">name or description</param>
        public Identifier(Type type, string name)
            : this(Local + "_" + type.FullName, GetNewID(), name, null, null, null)
        { }

        /// <summary>
        /// Creates a complete identifier.
        /// </summary>
        /// <param name="type">the class of the identified object</param>
        /// <param name="name">name or description</param>
        /// <param name="shortName">short name used for user interfaces</param>
        public Identifier(Type type, string name, string shortName)
             : this(Local + "_" + type.FullName, GetNewID(), name, shortName, null, null)
        { }

        /// <summary>
        /// Creates a complete identifier.
        /// </summary>
        /// <param name="type">the class of the identified object</param>
        /// <param name="name">name or description</param>
        /// <param name="shortName">short name used for user interfaces</param>
        /// <param name="aliases">synonyms of this Identifiable</param>
        public Identifier(Type type, string name, string shortName, List<Identifier> aliases)
            : this(Local + "_" + type.FullName, GetNewID(), name, shortName, null, aliases)
        { }

        /// <summary>
        /// Creates a complete identifier.
        /// </summary>
        /// <param name="authorityName">namespace of the identifier ie EPSG, IGNF</param>
        /// <param name="authorityKey">unique key in the namespace</param>
        /// <param name="name">name or description</param>
        public Identifier(string authorityName, string authorityKey, string name)
            : this(authorityName, authorityKey, name, null, null, null)
        { }

        /// <summary>
        /// Creates a complete identifier.
        /// </summary>
        /// <param name="authorityName">namespace of the identifier ie EPSG, IGNF</param>
        /// <param name="authorityKey">unique key in the namespace</param>
        /// <param name="name">name or description</param>
        /// <param name="shortName">short name used for user interfaces</param>
        public Identifier(string authorityName, string authorityKey, string name, string shortName)
            : this(authorityName, authorityKey, name, shortName, null, null)
        { }

        /// <summary>
        /// Creates a complete identifier.
        /// </summary>
        /// <param name="authorityName">namespace of the identifier ie EPSG, IGNF</param>
        /// <param name="authorityKey">unique key in the namespace</param>
        /// <param name="name">name or description</param>
        /// <param name="shortName">short name used for user interfaces</param>
        /// <param name="remarks">remarks containing additionnal information on the object</param>
        /// <param name="aliases">synonyms of this Identifiable</param>
        public Identifier(string authorityName, string authorityKey, string name, string shortName, string remarks, List<Identifier> aliases)
        {
            AuthorityName = authorityName;
            AuthorityKey = authorityKey;
            Name = name;
            ShortName = shortName;
            Remarks = remarks;
            Aliases = aliases;
        }
        #endregion

        #region properties
        /// <summary>
        /// Namespace of the identifier ie EPSG, IGNF
        /// </summary>
        [JsonProperty]
        public string AuthorityName { get; set; }

        /// <summary>
        /// Unique key in the namespace
        /// </summary>
        [JsonProperty]
        public string AuthorityKey { get; set; }

        /// <summary>
        /// Name or description
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short name used for user interfaces
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Remarks containing additionnal information on the object
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Synonyms of this Identifiable
        /// </summary>
        public List<Identifier> Aliases { get; set; }
        #endregion

        /// <summary>
        /// Returns a code formed with a namespace, ':' and the id value of identifier(ex.EPSG:27572).
        /// </summary>
        /// <returns>A String of the form namespace:identifier</returns>
        public string GetCode()
        {
            return AuthorityName + ":" + AuthorityKey;
        }

        /// <summary>
        /// Add remarks.
        /// </summary>
        /// <param name="remark">the remark to add to the Identifier's remarks</param>
        public void AddRemarks(string remark)
        {
            Remarks += "\n" + remark;
        }

        /// <summary>
        /// Add an alias
        /// </summary>
        /// <param name="identifier">an alias for this object</param>
        public void AddAlias(Identifier identifier)
        {
            Aliases.Add(identifier);
        }
    }
}
