using Dtos.Schedule;

namespace Services.Abstract
{
    public interface ICalService
    {
        void CreateAppointments(Agenda agenda);
    }
}
