/* Empiria Security Management *******************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Web Api                               *
*  Assembly : Empiria.Security.Management.WebApi.dll       Pattern   : Query Controller                      *
*  Type     : SubjectsController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api methods for subjects management.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Security.Subjects.Adapters;
using Empiria.Security.Subjects.UseCases;

namespace Empiria.Security.Management.WebApi {

  /// <summary>Web api methods for subjects management.</summary>
  public class SubjectsController : WebApiController {

    #region Query apis

    [HttpPost, AllowAnonymous]
    [Route("v5/security/management/new-credentials-token")]
    public SingleObjectModel GenerateNewCredentialsToken([FromBody] UserCredentialsDto credentials) {

      PrepareAuthenticationFields(credentials);

      using (var usecases = SubjectCredentialsUseCases.UseCaseInteractor()) {
        string token = usecases.GenerateNewCredentialsToken(credentials);

        return new SingleObjectModel(base.Request, token);
      }
    }


    [HttpPost]
    [Route("v5/security/management/subjects/search")]
    public CollectionModel GetSubjects([FromBody] SubjectsQuery query) {

      RequireBody(query);

      using (var usecases = SubjectUseCases.UseCaseInteractor()) {
        FixedList<SubjectDto> subjects = usecases.SearchSubjects(query);

        return new CollectionModel(base.Request, subjects);
      }
    }


    [HttpGet]
    [Route("v5/security/management/subjects/workareas")]
    public CollectionModel GetWorkareas() {

      using (var usecases = SubjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> workareas = usecases.Workareas();

        return new CollectionModel(base.Request, workareas);
      }
    }

    #endregion Query apis

    #region Command apis

    [HttpPost]
    [Route("v5/security/management/subjects/{subjectUID:guid}/activate")]
    public SingleObjectModel ActivateSubject([FromUri] string subjectUID) {

      using (var usecases = SubjectUseCases.UseCaseInteractor()) {
        SubjectDto subject = usecases.ActivateSubject(subjectUID);

        return new SingleObjectModel(base.Request, subject);
      }
    }


    [HttpPost]
    [Route("v5/security/management/subjects")]
    public SingleObjectModel CreateSubject([FromBody] SubjectFields fields) {

      base.RequireBody(fields);

      using (var usecases = SubjectUseCases.UseCaseInteractor()) {
        SubjectDto subject = usecases.CreateSubject(fields);

        return new SingleObjectModel(base.Request, subject);
      }
    }


    [HttpDelete]
    [Route("v5/security/management/subjects/{subjectUID:guid}")]
    public NoDataModel RemoveSubject([FromUri] string subjectUID) {

      using (var usecases = SubjectUseCases.UseCaseInteractor()) {
        usecases.RemoveSubject(subjectUID);

        return new NoDataModel(base.Request);
      }
    }



    [HttpPost]
    [Route("v5/security/management/subjects/{subjectUID:guid}/reset-credentials")]
    public NoDataModel ResetCredentials([FromUri] string subjectUID) {

      using (var usecases = SubjectCredentialsUseCases.UseCaseInteractor()) {
        usecases.ResetCredentials(subjectUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPost]
    [Route("v5/security/management/subjects/{subjectUID:guid}/suspend")]
    public SingleObjectModel SuspendSubject([FromUri] string subjectUID) {

      using (var usecases = SubjectUseCases.UseCaseInteractor()) {
        SubjectDto subject = usecases.SuspendSubject(subjectUID);

        return new SingleObjectModel(base.Request, subject);
      }
    }


    [HttpPost, AllowAnonymous]
    [Route("v5/security/management/update-my-credentials")]
    public NoDataModel UpdateCredentials([FromBody] UpdateCredentialsFields fields) {

      PrepareUpdateCredentialsFields(fields);

      using (var usecases = SubjectCredentialsUseCases.UseCaseInteractor()) {
        usecases.UpdateCredentials(fields);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v5/security/management/subjects/{subjectUID:guid}")]
    public SingleObjectModel UpdateSubject([FromUri] string subjectUID,
                                           [FromBody] SubjectFields fields) {

      base.RequireBody(fields);

      using (var usecases = SubjectUseCases.UseCaseInteractor()) {
        SubjectDto subject = usecases.UpdateSubject(subjectUID, fields);

        return new SingleObjectModel(base.Request, subject);
      }
    }

    #endregion Command apis

    #region Helpers

    private void PrepareAuthenticationFields(UserCredentialsDto credentials) {
      base.RequireHeader("User-Agent");
      base.RequireBody(credentials);

      credentials.AppKey = base.GetRequestHeader<string>("ApplicationKey");
      credentials.UserHostAddress = base.GetClientIpAddress();
    }

    private void PrepareUpdateCredentialsFields(UpdateCredentialsFields fields) {
      base.RequireBody(fields);

      fields.AppKey = base.GetRequestHeader<string>("ApplicationKey");
      fields.UserHostAddress = base.GetClientIpAddress();
    }

    #endregion Helpers

  }  // class SubjectsController

}  // namespace Empiria.Security.Management.WebApi
