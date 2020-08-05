using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using MediatR;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.ContactUsPages;

namespace Sandbox.Delivery.Web.Features.ContactUs
{
    public class ContactUsPageRequestHandler : IRequestHandler<ContactUsPageRequest, Maybe<ContactUsPageViewModel>>
    {
        private readonly IQueryDispatcher dispatcher;

        public ContactUsPageRequestHandler(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        public async Task<Maybe<ContactUsPageViewModel>> Handle(ContactUsPageRequest request, CancellationToken cancellationToken)
        {
            var result = await dispatcher.Dispatch(new ContactUsPageQuery(request.RequestPath), cancellationToken);

            if (result.IsFailure)
            {
                return Maybe<ContactUsPageViewModel>.None;
            }

            var response = result.Value;

            return new ContactUsPageViewModel(response.HeaderText);
        }
    }

    public class ContactUsPageRequest : IRequest<Maybe<ContactUsPageViewModel>>
    {
        public ContactUsPageRequest(string requestPath) => RequestPath = requestPath ?? "";

        public string RequestPath { get; }
    }

    public class ContactUsPageViewModel
    {
        public ContactUsPageViewModel(string title) => Title = title ?? "";

        public string Title { get; }
    }
}
