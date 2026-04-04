// using System.Collections.Immutable;
// using System.Reflection;
// using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
// using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;
// using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
// using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using SharedKernel.common;
// using SharedKernel.common.vokis;
// using SharedKernel.common.vokis.general_vokis;
// using SharedKernel.domain.ids;
// using SharedKernel.errs;
// using VokiCreationServicesLib.Domain.draft_voki_aggregate;
//
// namespace DbSeeder.seeding.newtonsoft;
//
// public class DraftGeneralVokiConverter : JsonConverter<DraftGeneralVoki>
// {
//     public override DraftGeneralVoki ReadJson(
//         JsonReader reader,
//         Type objectType,
//         DraftGeneralVoki? existingValue,
//         bool hasExistingValue,
//         JsonSerializer serializer
//     ) {
//         JObject jo = JObject.Load(reader);
//         var obj = (DraftGeneralVoki)JsonUtil.CreateUninitialized(typeof(DraftGeneralVoki));
//
//         if (jo.TryGetValue("name", out var jName)) {
//             JsonUtil.SetProperty(obj, "Name", new VokiName(jName.Value<string>()!));
//         }
//
//         JsonUtil.SetProperty(obj, "CoAuthors", VokiCoAuthorIdsSet.Empty);
//
//         if (jo.TryGetValue("details", out var jDetailsToken)) {
//             JObject jDetails = (JObject)jDetailsToken;
//
//             var description = VokiDescription.Create(
//                 jDetails["description"]!.Value<string>()!
//             ).AsSuccess();
//
//             bool hasMature = jDetails["hasMatureContent"]!.Value<bool>();
//
//             var lang = Enum.Parse<Language>(
//                 jDetails["language"]!.Value<string>()!,
//                 ignoreCase: true
//             );
//
//             JsonUtil.SetProperty(
//                 obj,
//                 "Details",
//                 new VokiDetails(description, hasMature, lang)
//             );
//         }
//
//         if (jo.TryGetValue("tags", out var jTags)) {
//             var arr = jTags.ToObject<List<string>>()!;
//             var tagSet = arr.Select(s => new VokiTagId(s)).ToImmutableHashSet();
//             JsonUtil.SetProperty(obj, "Tags", VokiTagsSet.Create(tagSet).AsSuccess());
//         }
//
//         if (jo.TryGetValue("takingProcessSettings", out var jTake)) {
//             var tps = jTake.ToObject<VokiTakingProcessSettings>(serializer);
//             JsonUtil.SetProperty(obj, "TakingProcessSettings", tps);
//         }
//
//         JObject jIntSet = (JObject)jo["interactionSettings"]!;
//         bool signedInOnly = jIntSet["signedInOnlyTaking"]!.Value<bool>();
//         var visibility = (GeneralVokiResultsVisibility)jIntSet["resultsVisibility"]!.Value<int>();
//         bool showDistribution = jIntSet["showResultsDistribution"]!.Value<bool>();
//
//         JsonUtil.SetProperty(
//             obj, "InteractionSettings",
//             GeneralVokiInteractionSettings
//                 .Create(signedInOnly, visibility, showDistribution)
//                 .AsSuccess()
//         );
//
//         FieldInfo questionsField = obj.GetType()
//             .GetField("_questions", BindingFlags.Instance | BindingFlags.NonPublic)!;
//
//         List<VokiQuestion> questionsList = [];
//         foreach (var jQ in (jo["questions"] as JArray)!) {
//             var q = DeserializeQuestion((JObject)jQ, serializer);
//             questionsList.Add(q);
//         }
//
//         questionsField.SetValue(obj, questionsList);
//
//         FieldInfo resultsField = obj.GetType()
//             .GetField("_results", BindingFlags.Instance | BindingFlags.NonPublic)!;
//
//         List<VokiResult> resultsList = [];
//         foreach (var jR in (jo["results"] as JArray)!) {
//             var r = DeserializeResult((JObject)jR);
//             resultsList.Add(r);
//         }
//
//         resultsField.SetValue(obj, resultsList);
//
//         return obj;
//     }
//
//     private VokiQuestion DeserializeQuestion(JObject jo, JsonSerializer serializer) {
//         var q = (VokiQuestion)JsonUtil.CreateUninitialized(typeof(VokiQuestion));
//         GeneralVokiAnswerType answerType = Enum.Parse<GeneralVokiAnswerType>(jo["answersType"]!.Value<string>()!, true);
//
//         
//         JsonUtil.SetProperty(q, "Text", VokiQuestionText.Create(jo["text"]!.Value<string>()!).AsSuccess());
//         JsonUtil.SetProperty(q, "OrderInVoki", jo["orderInVoki"]!.Value<ushort>());
//         JsonUtil.SetProperty(q, "ShuffleAnswers", jo["shuffleAnswers"]!.Value<bool>());
//         JsonUtil.SetProperty(q, "ImageSet", VokiQuestionImagesSet.Default);
//         JsonUtil.SetProperty(q, "AnswersType", answerType);
//         
//         if (jo.TryGetValue("answersCountLimit", out var jLimitToken)) {
//             var jLimit = (JObject)jLimitToken;
//             ushort min = jLimit["min"]!.Value<ushort>();
//             ushort max = jLimit["max"]!.Value<ushort>();
//             JsonUtil.SetProperty(q, "AnswersCountLimit",
//                 QuestionAnswersCountLimit.MultipleChoice(min, max).AsSuccess());
//         }
//         
//         List<VokiQuestionAnswer> answersList = [];
//         ushort ansOrder = 1;
//
//         foreach (var jA in (jo["answers"] as JArray)!) {
//             answersList.Add(DeserializeAnswer((JObject)jA, ansOrder, answerType));
//             ansOrder++;
//         }
//
//         q.GetType()
//             .GetField("_answers", BindingFlags.NonPublic | BindingFlags.Instance)!
//             .SetValue(q, answersList);
//
//         return q;
//     }
//
//     private VokiQuestionAnswer DeserializeAnswer(JObject jo, ushort order, GeneralVokiAnswerType t) {
//         var a = (VokiQuestionAnswer)JsonUtil.CreateUninitialized(typeof(VokiQuestionAnswer));
//
//         JsonUtil.SetProperty(a, "OrderInQuestion", order);
//         BaseVokiAnswerTypeData typeData = t.Match<BaseVokiAnswerTypeData>(
//             textOnly: () =>
//                 new BaseVokiAnswerTypeData.TextOnly(
//                     GeneralVokiAnswerText.Create(jo["text"]!.Value<string>()!).AsSuccess()
//                 ),
//             imageOnly: () => throw new Exception("Cannot deserialize image only answer type"),
//             imageAndText: () => throw new Exception("Cannot deserialize image and text  answer type"),
//             colorOnly: () =>
//                 new BaseVokiAnswerTypeData.ColorOnly(
//                     new HexColor(jo["color"]!.Value<string>()!)
//                 ),
//             colorAndText: () =>
//                 new BaseVokiAnswerTypeData.ColorAndText(
//                     GeneralVokiAnswerText.Create(jo["text"]!.Value<string>()!).AsSuccess(),
//                     HexColor.Create(jo["color"]!.Value<string>()!).AsSuccess()
//                 ),
//             () => throw new Exception("Cannot deserialize audio only answer type"),
//             () => throw new Exception("Cannot deserialize audio and text answer type")
//         );
//
//         JsonUtil.SetProperty(a, "TypeData", typeData);
//
//         List<string> res = jo["relatedResults"]!.ToObject<List<string>>()!;
//         var resIds = res.Select(s => new GeneralVokiResultId(Guid.Parse(s))).ToImmutableHashSet();
//         JsonUtil.SetProperty(a, "RelatedResultIds", resIds);
//
//         return a;
//     }
//
//     private VokiResult DeserializeResult(JObject jo) {
//         var r = (VokiResult)JsonUtil.CreateUninitialized(typeof(VokiResult));
//         JsonUtil.SetProperty(
//             r,
//             "Id",
//             new GeneralVokiResultId(new Guid(jo["id"]!.Value<string>()!))
//         );
//         JsonUtil.SetProperty(
//             r,
//             "Name",
//             VokiResultName.Create(jo["name"]!.Value<string>()!).AsSuccess()
//         );
//         JsonUtil.SetProperty(
//             r,
//             "Text",
//             VokiResultText.Create(jo["text"]!.Value<string>()!).AsSuccess()
//         );
//
//         return r;
//     }
//
//     public override void WriteJson(JsonWriter writer, DraftGeneralVoki? value, JsonSerializer serializer)
//         => throw new NotImplementedException();
// }