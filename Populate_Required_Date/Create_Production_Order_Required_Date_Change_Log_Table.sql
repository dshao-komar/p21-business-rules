IF OBJECT_ID('dbo.prod_order_required_date_change_log', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.prod_order_required_date_change_log
    (
        prod_order_required_date_change_log_uid int IDENTITY(1, 1) NOT NULL,
        logged_at datetime NOT NULL
            CONSTRAINT DF_prod_order_required_date_change_log_logged_at DEFAULT (GETDATE()),
        job_name varchar(128) NOT NULL
            CONSTRAINT DF_prod_order_required_date_change_log_job_name DEFAULT ('Populate_Required_Date nightly sync'),
        prod_order_number decimal(19, 0) NOT NULL,
        sales_order_no varchar(50) NULL,
        old_required_date date NULL,
        new_required_date date NULL,
        old_customer_name varchar(255) NULL,
        new_customer_name varchar(255) NULL,
        CONSTRAINT PK_prod_order_required_date_change_log
            PRIMARY KEY CLUSTERED (prod_order_required_date_change_log_uid)
    );

    CREATE INDEX IX_prod_order_required_date_change_log_prod_order_number
        ON dbo.prod_order_required_date_change_log (prod_order_number, logged_at DESC);
END;
