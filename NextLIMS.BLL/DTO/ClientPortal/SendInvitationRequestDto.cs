using System.ComponentModel.DataAnnotations;

namespace NextLIMS.BLL.DTO.ClientPortal
{
    public class SendInvitationRequestDto
    {
        [Range(1, int.MaxValue)]
        public int SampleId { get; set; }
    }
}