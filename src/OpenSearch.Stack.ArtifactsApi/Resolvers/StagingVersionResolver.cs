/* SPDX-License-Identifier: Apache-2.0
*
* The OpenSearch Contributors require contributions made to
* this file be licensed under the Apache-2.0 license or a
* compatible open source license.
*
* Modifications Copyright OpenSearch Contributors. See
* GitHub history for details.
*
*  Licensed to Elasticsearch B.V. under one or more contributor
*  license agreements. See the NOTICE file distributed with
*  this work for additional information regarding copyright
*  ownership. Elasticsearch B.V. licenses this file to you under
*  the Apache License, Version 2.0 (the "License"); you may
*  not use this file except in compliance with the License.
*  You may obtain a copy of the License at
*
* 	http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing,
*  software distributed under the License is distributed on an
*  "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
*  KIND, either express or implied.  See the License for the
*  specific language governing permissions and limitations
*  under the License.
*/

using OpenSearch.Stack.ArtifactsApi.Platform;
using OpenSearch.Stack.ArtifactsApi.Products;
using SemVer;

namespace OpenSearch.Stack.ArtifactsApi.Resolvers
{
	public static class StagingVersionResolver
	{
		// TODO: update string when working on artifacts API
		private const string StagingUrlFormat = "https://staging.opensearch.org/{0}-{1}";

		public static bool TryResolve(Product product, Version version, string buildHash, out Artifact artifact)
		{
			artifact = null;
			if (string.IsNullOrWhiteSpace(buildHash)) return false;

			var p = product.Moniker;
			var stagingRoot = string.Format(StagingUrlFormat, version, buildHash);
			var archive = $"{p}-{version}-{OsMonikers.CurrentPlatformPackageSuffix()}.{product.Extension}";
			if (!product.PlatformDependent || version <= product.PlatformSuffixAfter)
				archive = $"{p}-{version}.{product.Extension}";

			var downloadUrl = $"{stagingRoot}/downloads/{product}/{archive}";
			artifact = new Artifact(product, version, downloadUrl, ArtifactBuildState.BuildCandidate, buildHash);
			return true;
		}
	}
}