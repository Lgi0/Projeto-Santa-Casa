USE [master]
GO
CREATE DATABASE [ProjetoDB]
GO

USE [ProjetoDB]
GO


CREATE TABLE [dbo].[paciente](
	[id_paciente] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](100) NOT NULL,
	[nascimento] [date] NOT NULL,
	[cpf] [varchar](11) NOT NULL,
	[telefone] [varchar](15) NOT NULL,
	[email] [varchar](100) NULL,
	[endereco] [varchar](100) NOT NULL,
	[esf] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Internacoes](
	[id_internacao] [int] IDENTITY(1,1) NOT NULL,
	[id_paciente] [int] NOT NULL,
	[setor] [nvarchar](50) NOT NULL,
	[leito] [nvarchar](20) NOT NULL,
	[data_internacao] [date] NOT NULL,
	[data_alta] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_internacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Internacoes]  WITH NOCHECK ADD FOREIGN KEY([id_paciente])
REFERENCES [dbo].[paciente] ([id_paciente])


GO


CREATE TABLE [dbo].[pts](
	[id_pts] [int] IDENTITY(1,1) NOT NULL,
	[id_paciente] [int] NOT NULL,
	[enfermeiro] [varchar](100) NULL,
	[medica] [varchar](100) NULL,
	[nutricionista] [varchar](100) NULL,
	[fisioterapeuta] [varchar](100) NULL,
	[psicologa] [varchar](100) NULL,
	[medicamentos] [text] NULL,
	[historia] [text] NULL,
	[enfermagem] [text] NULL,
	[fisioterapia] [text] NULL,
	[nutricao] [text] NULL,
	[curto_prazo] [text] NULL,
	[medio_prazo] [text] NULL,
	[longo_prazo] [text] NULL,
	[data_criacao] [datetime] NULL,
	[pa] [varchar](200) NULL,
	[inspecao] [text] NULL,
	[avaliacao_fisica] [text] NULL,
	[grau_mobilidade] [text] NULL,
	[forca_sensibilidade] [text] NULL,
	[nivel_dependencia] [text] NULL,
	[id_internacao] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_pts] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[pts] ADD  DEFAULT (getdate()) FOR [data_criacao]
GO

ALTER TABLE [dbo].[pts]  WITH NOCHECK ADD FOREIGN KEY([id_paciente])
REFERENCES [dbo].[paciente] ([id_paciente])
GO

ALTER TABLE [dbo].[pts]  WITH NOCHECK ADD  CONSTRAINT [FK_pts_internacao] FOREIGN KEY([id_internacao])
REFERENCES [dbo].[Internacoes] ([id_internacao])
GO

ALTER TABLE [dbo].[pts] CHECK CONSTRAINT [FK_pts_internacao]
GO

CREATE TABLE [dbo].[pta_enfermagem](
	[id_pta_enfermagem] [int] IDENTITY(1,1) NOT NULL,
	[id_paciente] [int] NOT NULL,
	[crm] [nvarchar](30) NULL,
	[hd] [nvarchar](255) NULL,
	[portador_diabetes] [bit] NULL,
	[portador_has] [bit] NULL,
	[portador_clinico] [bit] NULL,
	[portador_cirurgico] [bit] NULL,
	[portador_outros] [nvarchar](255) NULL,
	[glasgow] [nvarchar](50) NULL,
	[necessita_ulcera] [bit] NULL,
	[necessita_estomas] [bit] NULL,
	[necessita_sonda] [bit] NULL,
	[necessita_traqueo] [bit] NULL,
	[necessita_oxigenio] [bit] NULL,
	[necessita_aspiracao] [bit] NULL,
	[necessita_curativos] [bit] NULL,
	[necessita_outros] [nvarchar](255) NULL,
	[orientacao_paciente] [bit] NULL,
	[orientacao_familiar] [bit] NULL,
	[orientacao_outro] [bit] NULL,
	[tipo_ori_curativos] [bit] NULL,
	[tipo_ori_prevencao_ulcera] [bit] NULL,
	[tipo_ori_sondas] [bit] NULL,
	[tipo_ori_aspiracao] [bit] NULL,
	[tipo_ori_dieta] [bit] NULL,
	[tipo_ori_pele] [bit] NULL,
	[descricao] [nvarchar](max) NULL,
	[dados_iniciais] [nvarchar](max) NULL,
	[dados_internacao] [nvarchar](max) NULL,
	[dados_alta] [nvarchar](max) NULL,
	[data_criacao] [datetime] NULL,
	[id_internacao] [int] NOT NULL,
	[medico] [nvarchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_pta_enfermagem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[pta_enfermagem] ADD  DEFAULT (getdate()) FOR [data_criacao]
GO

ALTER TABLE [dbo].[pta_enfermagem]  WITH NOCHECK ADD FOREIGN KEY([id_paciente])
REFERENCES [dbo].[paciente] ([id_paciente])
GO

ALTER TABLE [dbo].[pta_enfermagem]  WITH NOCHECK ADD  CONSTRAINT [FK_pta_enfermagem_internacao] FOREIGN KEY([id_internacao])
REFERENCES [dbo].[Internacoes] ([id_internacao])
GO

ALTER TABLE [dbo].[pta_enfermagem] CHECK CONSTRAINT [FK_pta_enfermagem_internacao]
GO


CREATE TABLE [dbo].[grupo_usuario](
	[id_gp_usuario] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](50) NULL,
	[descricao] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_gp_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[CodigoRedefinicao](
	[Telefone] [varchar](20) NOT NULL,
	[Codigo] [varchar](10) NOT NULL,
	[DataExpiracao] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Telefone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[alta](
	[id_alta] [int] IDENTITY(1,1) NOT NULL,
	[id_paciente] [int] NULL,
	[data_inicio] [datetime] NOT NULL,
	[data_fim] [datetime] NOT NULL,
	[setor] [varchar](50) NOT NULL,
	[leito] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_alta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[alta]  WITH CHECK ADD FOREIGN KEY([id_paciente])
REFERENCES [dbo].[paciente] ([id_paciente])

GO