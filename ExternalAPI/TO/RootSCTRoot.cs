using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace ExternalAPI.TO
{

    public partial class RootSCT
    {
        [JsonProperty("eventId")]
        public long? EventId { get; set; }

        [JsonProperty("eventTypeId")]
        public long? EventTypeId { get; set; }

        [JsonProperty("marketId")]
        public string MarketId { get; set; }

        [JsonProperty("marketName")]
        public string MarketName { get; set; }

        [JsonProperty("relevance")]
        public long? Relevance { get; set; }

        [JsonProperty("eventName")]
        public string EventName { get; set; }

        [JsonProperty("competitionName")]
        public string CompetitionName { get; set; }

        [JsonProperty("competitionId")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? CompetitionId { get; set; }

        [JsonProperty("numberOfRunners")]
        public long? NumberOfRunners { get; set; }

        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("runners")]
        public Runners Runners { get; set; }

        [JsonProperty("homeName")]
        public string HomeName { get; set; }

        [JsonProperty("awayName")]
        public string AwayName { get; set; }

        [JsonProperty("broadcasts", NullValueHandling = NullValueHandling.Ignore)]
        public Broadcasts Broadcasts { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("matchInfo")]
        public MatchInfo MatchInfo { get; set; }

        [JsonProperty("inPlayBettingStatus")]
        public string InPlayBettingStatus { get; set; }

        [JsonProperty("countryCode", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCode { get; set; }
    }

    public partial class Broadcasts
    {
        [JsonProperty("tv")]
        public Tv[] Tv { get; set; }

        [JsonProperty("radio")]
        public Radio Radio { get; set; }

        [JsonProperty("bfLiveVideo")]
        public MatchInfo BfLiveVideo { get; set; }

        [JsonProperty("isLiveVideoAvailable")]
        public bool? IsLiveVideoAvailable { get; set; }

        [JsonProperty("isDataVisualizationAvailable")]
        public bool? IsDataVisualizationAvailable { get; set; }

        [JsonProperty("isPaddockViewAvailable")]
        public bool? IsPaddockViewAvailable { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
    }

    public partial class MatchInfo
    {
    }

    public partial class Radio
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }

    public partial class Tv
    {
        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTimeOffset EndTime { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
    }

    public partial class Runners
    {
        [JsonProperty("runner1Name")]
        public string Runner1Name { get; set; }

        [JsonProperty("runner2Name")]
        public string Runner2Name { get; set; }

        [JsonProperty("drawName")]
        public string DrawName { get; set; }

        [JsonProperty("runner1SelectionId")]
        public long? Runner1SelectionId { get; set; }

        [JsonProperty("runner2SelectionId")]
        public long? Runner2SelectionId { get; set; }

        [JsonProperty("drawSelectionId")]
        public long? DrawSelectionId { get; set; }
    }

    public partial class State
    {
        [JsonProperty("eventTypeId")]
        public long? EventTypeId { get; set; }

        [JsonProperty("eventId")]
        public long? EventId { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

        [JsonProperty("timeElapsed")]
        public long? TimeElapsed { get; set; }

        [JsonProperty("elapsedRegularTime")]
        public long? ElapsedRegularTime { get; set; }

        [JsonProperty("elapsedAddedTime", NullValueHandling = NullValueHandling.Ignore)]
        public long? ElapsedAddedTime { get; set; }

        [JsonProperty("timeElapsedSeconds")]
        public long? TimeElapsedSeconds { get; set; }

        [JsonProperty("fullTimeElapsed")]
        public FullTimeElapsed FullTimeElapsed { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("matchStatus")]
        public string MatchStatus { get; set; }

        [JsonProperty("hasSets", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasSets { get; set; }
    }

    public partial class FullTimeElapsed
    {
        [JsonProperty("hour")]
        public long? Hour { get; set; }

        [JsonProperty("min")]
        public long? Min { get; set; }

        [JsonProperty("sec")]
        public long? Sec { get; set; }
    }

    public partial class Score
    {
        [JsonProperty("home")]
        public Away Home { get; set; }

        [JsonProperty("away")]
        public Away Away { get; set; }

        [JsonProperty("numberOfYellowCards")]
        public long? NumberOfYellowCards { get; set; }

        [JsonProperty("numberOfRedCards")]
        public long? NumberOfRedCards { get; set; }

        [JsonProperty("numberOfCards")]
        public long? NumberOfCards { get; set; }

        [JsonProperty("numberOfCorners")]
        public long? NumberOfCorners { get; set; }

        [JsonProperty("gameSequence")]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public long[] GameSequence { get; set; }

        [JsonProperty("numberOfCornersFirstHalf")]
        public long NumberOfCornersFirstHalf { get; set; }

        [JsonProperty("numberOfCornersSecondHalf", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumberOfCornersSecondHalf { get; set; }

        [JsonProperty("bookingPoints")]
        public long? BookingPoints { get; set; }

        [JsonProperty("serviceBreaks")]
        public long? ServiceBreaks { get; set; }
    }

    public partial class Away
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? Score { get; set; }

        [JsonProperty("halfTimeScore")]
        public string HalfTimeScore { get; set; }

        [JsonProperty("fullTimeScore")]
        public string FullTimeScore { get; set; }

        [JsonProperty("penaltiesScore")]
        public string PenaltiesScore { get; set; }

        [JsonProperty("penaltiesSequence")]
        public object[] PenaltiesSequence { get; set; }

        [JsonProperty("games")]
        public string Games { get; set; }

        [JsonProperty("sets")]
        public string Sets { get; set; }

        [JsonProperty("numberOfYellowCards")]
        public long? NumberOfYellowCards { get; set; }

        [JsonProperty("numberOfRedCards")]
        public long? NumberOfRedCards { get; set; }

        [JsonProperty("numberOfCards")]
        public long? NumberOfCards { get; set; }

        [JsonProperty("numberOfCorners")]
        public long? NumberOfCorners { get; set; }

        [JsonProperty("gameSequence")]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public long[] GameSequence { get; set; }

        [JsonProperty("numberOfCornersFirstHalf")]
        public long? NumberOfCornersFirstHalf { get; set; }

        [JsonProperty("numberOfCornersSecondHalf", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumberOfCornersSecondHalf { get; set; }

        [JsonProperty("bookingPoints")]
        public long? BookingPoints { get; set; }

        [JsonProperty("highlight", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Highlight { get; set; }

        [JsonProperty("serviceBreaks")]
        public long? ServiceBreaks { get; set; }
    }
    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            //throw new Exception("Cannot unmarshal type long");
            return "";
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class DecodeArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long[]);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new List<long>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                var converter = ParseStringConverter.Singleton;
                var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                value.Add(arrayItem);
                reader.Read();
            }
            return value.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (long[])untypedValue;
            writer.WriteStartArray();
            foreach (var arrayItem in value)
            {
                var converter = ParseStringConverter.Singleton;
                converter.WriteJson(writer, arrayItem, serializer);
            }
            writer.WriteEndArray();
            return;
        }

        public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
    }

}


