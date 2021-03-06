
Drop table DrugProject;
Drop table ProjectLog;
Drop table DrugProjectItem;
Drop table Tongji;

CREATE TABLE DrugProject(
   ID         INT PRIMARY KEY    NOT NULL,
   DurgName   VARCHAR(100)  NOT NULL,
   AddDate    VARCHAR(100)  NOT NULL,
   LastDate   VARCHAR(100)  NOT NULL
);


CREATE TABLE  DrugProjectItem(
   ID         INT PRIMARY KEY  NOT NULL,
   DrugProjectID  INT   NOT NULL,
   CodeNum1   VARCHAR(100)  NOT NULL,
   CodeNum2   VARCHAR(100)  NOT NULL,
   PJSFMJ    VARCHAR(100)  NOT NULL,
   GSCYL     VARCHAR(100)  NOT NULL,
   XSBS      VARCHAR(100)  NOT NULL,
   DZLD      VARCHAR(100)  NOT NULL,
   DZFMJ     VARCHAR(100)  NOT NULL,
   HL        VARCHAR(100)  ,
   PJHL      VARCHAR(100)  ,
   FC        VARCHAR(100)  ,
   IsFuCe    VARCHAR(10)  NOT NULL,
   Type      VARCHAR(10)  NOT NULL
);


CREATE TABLE  ProjectLog(
   ID              INT PRIMARY KEY  NOT NULL,
   DrugProjectID   INT   NOT NULL,
   OpType          VARCHAR(20)  NOT NULL,
   BeforeValue     VARCHAR(1000)  NOT NULL,
   AfterValue      VARCHAR(1000)  NOT NULL,
   ChangeDate      VARCHAR(100)  NOT NULL
);


CREATE TABLE  Tongji(
   ID              INT PRIMARY KEY  NOT NULL,
   ProjectID       INT   NOT NULL,
   GroupName       VARCHAR(100)  NOT NULL,
   TJHL            VARCHAR(100)  NOT NULL,
   YPHL            VARCHAR(100)  NOT NULL,
   ZYLv            VARCHAR(100)  NOT NULL
);
