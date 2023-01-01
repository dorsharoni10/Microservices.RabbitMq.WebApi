using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Publisher.Controller;

public class TicketController : ControllerBase{
    private readonly IBus _bus;
    public TicketController(IBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(Ticket ticket)
    {
        if(ticket == null){
            return BadRequest();
        }

        ticket.BookedOn = DateTime.Now;
        
        var uri = new Uri("rabbitmq://localhost/ticketQueue");
        var endPoint = await _bus.GetSendEndpoint(uri);
        await endPoint.Send(ticket);
        return Ok("Success");
    }
}