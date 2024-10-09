/* Empiria Security Management *******************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Web Api                               *
*  Assembly : Empiria.Security.Management.WebApi.dll       Pattern   : Query Controller                      *
*  Type     : SubjectSecurityItemsController               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api methods for subjects assigned security items in a given execution context.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Security.Subjects.UseCases;

namespace Empiria.Security.Management.WebApi {

  /// <summary>Web api methods for subjects assigned security items in a given execution context.</summary>
  public class SubjectSecurityItemsController : WebApiController {

    #region Query apis

    [HttpGet]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts")]
    public CollectionModel GetSubjectContexts([FromUri] string subjectUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> contexts = usecases.GetSubjectContexts(subjectUID);

        return new CollectionModel(base.Request, contexts);
      }
    }


    [HttpGet]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts/{contextUID:guid}/features")]
    public CollectionModel GetSubjectFeatures([FromUri] string subjectUID, [FromUri] string contextUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> features = usecases.GetSubjectFeatures(subjectUID, contextUID);

        return new CollectionModel(base.Request, features);
      }
    }


    [HttpGet]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts/{contextUID:guid}/roles")]
    public CollectionModel GetSubjectRoles([FromUri] string subjectUID, [FromUri] string contextUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> roles = usecases.GetSubjectRoles(subjectUID, contextUID);

        return new CollectionModel(base.Request, roles);
      }
    }

    #endregion Query apis

    #region Command apis

    [HttpPost]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts/{contextUID:guid}")]
    public CollectionModel AssignContextToSubject([FromUri] string subjectUID,
                                                  [FromUri] string contextUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        usecases.AssignContext(subjectUID, contextUID);

        FixedList<NamedEntityDto> contexts = usecases.GetSubjectContexts(subjectUID);

        return new CollectionModel(base.Request, contexts);
      }
    }


    [HttpPost]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts/{contextUID:guid}/features/{featureUID}")]
    public CollectionModel AssignFeatureToSubject([FromUri] string subjectUID,
                                                  [FromUri] string contextUID,
                                                  [FromUri] string featureUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        usecases.AssignFeature(subjectUID, contextUID, featureUID);

        FixedList<NamedEntityDto> features = usecases.GetSubjectFeatures(subjectUID, contextUID);

        return new CollectionModel(base.Request, features);
      }
    }


    [HttpPost]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts/{contextUID:guid}/roles/{roleUID:guid}")]
    public CollectionModel AssignRoleToSubject([FromUri] string subjectUID,
                                               [FromUri] string contextUID,
                                               [FromUri] string roleUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        usecases.AssignRole(subjectUID, contextUID, roleUID);

        FixedList<NamedEntityDto> roles = usecases.GetSubjectRoles(subjectUID, contextUID);

        return new CollectionModel(base.Request, roles);
      }
    }


    [HttpDelete]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts/{contextUID:guid}")]
    public CollectionModel UnsassignSubjectContext([FromUri] string subjectUID,
                                                   [FromUri] string contextUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        usecases.UnassignContext(subjectUID, contextUID);

        FixedList<NamedEntityDto> contexts = usecases.GetSubjectContexts(subjectUID);

        return new CollectionModel(base.Request, contexts);
      }
    }


    [HttpDelete]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts/{contextUID:guid}/features/{featureUID}")]
    public CollectionModel UnassignSubjectFeature([FromUri] string subjectUID,
                                                  [FromUri] string contextUID,
                                                  [FromUri] string featureUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        usecases.UnassignFeature(subjectUID, contextUID, featureUID);

        FixedList<NamedEntityDto> features = usecases.GetSubjectFeatures(subjectUID, contextUID);

        return new CollectionModel(base.Request, features);
      }
    }


    [HttpDelete]
    [Route("v5/security/management/subjects/{subjectUID:guid}/contexts/{contextUID:guid}/roles/{roleUID:guid}")]
    public CollectionModel UnassignSubjectRole([FromUri] string subjectUID,
                                               [FromUri] string contextUID,
                                               [FromUri] string roleUID) {

      using (var usecases = SubjectSecurityItemsUseCases.UseCaseInteractor()) {

        usecases.UnassignRole(subjectUID, contextUID, roleUID);

        FixedList<NamedEntityDto> roles = usecases.GetSubjectRoles(subjectUID, contextUID);

        return new CollectionModel(base.Request, roles);
      }
    }

    #endregion Command apis

  }  // class SubjectSecurityItemsController

}  // namespace Empiria.Security.Management.WebApi
