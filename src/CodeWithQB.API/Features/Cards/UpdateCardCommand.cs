using CodeWithQB.Core.Interfaces;
using CodeWithQB.Core.Models;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace CodeWithQB.API.Features.Cards
{
    public class UpdateCardCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Card.CardId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public CardDto Card { get; set; }
        }

        public class Response
        {            
            public Guid CardId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventStore _eventStore;
            
            public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var card = _eventStore.Load<Card>(request.Card.CardId);

                card.ChangeName(request.Card.Name);

                _eventStore.Save(card);

                return Task.FromResult(new Response() { CardId = request.Card.CardId }); 
            }
        }
    }
}
