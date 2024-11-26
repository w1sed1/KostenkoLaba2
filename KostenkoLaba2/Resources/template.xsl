<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" encoding="UTF-8" indent="yes" />

	<xsl:template match="/Results">
		<html>
			<head>
				<title>Результати пошуку</title>
				
			</head>
			<body>
				<h1>Результати пошуку</h1>
				<table>
					<tr>
						<th>ПІБ</th>
						<th>Факультет</th>
						<th>Курс</th>
						<th>Кімната</th>
						
					</tr>
					<xsl:for-each select="Person">
						<tr>
							<td>
								<xsl:value-of select="@FullName" />
							</td>
							<td>
								<xsl:value-of select="@Faculty" />
							</td>
							<td>
								<xsl:value-of select="@Course" />
							</td>
							<td>
								<xsl:value-of select="Room" />
							</td>
							
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
