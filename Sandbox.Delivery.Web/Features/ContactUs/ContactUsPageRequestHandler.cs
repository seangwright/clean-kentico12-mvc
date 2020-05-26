using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using MediatR;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.ContactUsPages;
using Sandbox.Delivery.Web.Infrastructure.PageBuilders;

namespace Sandbox.Delivery.Web.Features.ContactUs
{
    public class ContactUsPageRequestHandler : RequestHandler<ContactUsPageRequest, Maybe<ContactUsPageViewModel>>
    {
        private readonly IQueryDispatcher dispatcher;

        public ContactUsPageRequestHandler(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        protected override Maybe<ContactUsPageViewModel> Handle(ContactUsPageRequest request)
        {
            var result = dispatcher.Dispatch(new ContactUsPageQuery(request.RequestPath));

            if (result.IsFailure)
            {
                return Maybe<ContactUsPageViewModel>.None;
            }

            var response = result.Value;

            return new ContactUsPageViewModel(response.DocumentId, response.HeaderText);
        }
    }

    public class ContactUsPageRequest : IRequest<Maybe<ContactUsPageViewModel>>
    {
        public ContactUsPageRequest(string requestPath) => RequestPath = requestPath ?? "";

        public string RequestPath { get; }
    }

    public class ContactUsPageViewModel : IPageBuilderViewModel
    {
        public ContactUsPageViewModel(int documentId, string title)
        {
            DocumentId = documentId;
            Title = title ?? "";
        }

        public int DocumentId { get; }
        public string Title { get; }
    }
}
