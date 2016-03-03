using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace GameStore.WebUI.Components.AttributeAdapters
{
	public class LocalizedRegularExpressionAttributeAdapter : RegularExpressionAttributeAdapter
	{
		public LocalizedRegularExpressionAttributeAdapter(ModelMetadata metadata, ControllerContext context, RegularExpressionAttribute attribute)
			: base(metadata, context, attribute)
		{
			if (attribute.ErrorMessageResourceType == null)
			{
				attribute.ErrorMessageResourceType = typeof(GlobalResource);
			}

			if (string.IsNullOrEmpty(attribute.ErrorMessageResourceName))
			{
				attribute.ErrorMessageResourceName = "RegularExpressionAttribute_ValidationError";
			}
		}
	}
}