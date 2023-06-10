using Newtonsoft.Json;

namespace UnityGPT
{
    public class GptFineTunesRequest
    {
        [JsonProperty("training_file")] public string TrainingFileId { get; set; }
        [JsonProperty("model")] public string Model { get; set; }
    }
    
    public class GptFineTunesCancelRequest
    {
        [JsonProperty("fine_tune_id")] public string FineTuneId { get; set; }
    }
}