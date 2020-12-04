using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.model
{
    public class MessageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Data { get; set; }

        public string DataContentType { get; set; }

        public string CloudEventType { get; set; }

        public string CloudEventId { get; set; }

        public bool IsComplete { get; set; }
    }
}