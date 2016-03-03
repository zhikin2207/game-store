using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace GameStore.WebUI.Components.AttributeAdapters
{
	public class LocalizedStringLengthAttributeAdapter : StringLengthAttributeAdapter
	{
		public LocalizedStringLengthAttributeAdapter(ModelMetadata metadata, ControllerContext context, StringLengthAttribute attribute)
			: base(metadata, context, attribute)
		{
			if (attribute.ErrorMessageResourceType == null)
			{
				attribute.ErrorMessageResourceType = typeof(GlobalResource);
			}

			if (string.IsNullOrEmpty(attribute.ErrorMessageResourceName))
			{
				attribute.ErrorMessageResourceName = "StringLengthAttribute_ValidationError";
			}
		}
	}
}