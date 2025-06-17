using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementTool.Models
{
    public class TaskItem
    {
        //[JsonIgnore]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; private set; }

        [Required(ErrorMessage = "Titel darf nicht leer sein.")]
        [MinLength(3, ErrorMessage = "Titel muss mindestens 3 Zeichen haben.")]
        public string Titel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Beschreibumg darf nicht leer sein.")]
        public string Beschreibung { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priorität ist erforderlich.")]
        [EnumDataType(typeof(PrioritätValue), ErrorMessage = "Ungültige Priorität.")]
        public PrioritätValue Priorität { get; set; } //= PrioritätValue.Medium;

        [Required(ErrorMessage = "Status ist erforderlich.")]
        [EnumDataType(typeof(Status), ErrorMessage = "Ungültiger Status.")]
        public Status AktuellerStatus { get; set; } //= Status.Offen;

        internal void SetId(int id)
        {
            ID = id;
        }
    }



    public enum PrioritätValue
    {
        Low,
        Medium,
        High
    }

    public enum Status
    {
        Offen,
        InBearbeitung,
        Erledigt
    }


}
