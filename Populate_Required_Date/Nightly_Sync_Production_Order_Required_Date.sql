SET NOCOUNT ON;

WITH cte_open_sales_orders AS
(
    SELECT
          oel.inv_mast_uid
        , cst.customer_name
        , CAST(COALESCE(oels.release_date, oel.required_date) AS date) AS required_date
        , CAST(COALESCE(oels.release_qty, oel.qty_ordered) AS decimal(19, 6)) AS qty_ordered
        , oel.order_no
    FROM oe_line oel
    INNER JOIN oe_hdr oeh
        ON oeh.order_no = oel.order_no
    LEFT JOIN oe_line_schedule oels
        ON oels.order_no = oel.order_no
       AND oels.line_no = oel.line_no
    INNER JOIN customer cst
        ON cst.customer_id = oeh.customer_id
    WHERE oel.complete = 'N'
      AND oel.detail_type <> 1
      AND COALESCE(oeh.order_type, 0) NOT IN (1877, 1706)
      AND ISNULL(oeh.rma_flag, 'N') <> 'Y'
      AND ISNULL(oeh.warranty_rma_flag, 'N') <> 'Y'
      AND COALESCE(oels.release_date, oel.required_date) IS NOT NULL
),
cte_ranked_matches AS
(
    SELECT
          poh.prod_order_number
        , cso.required_date
        , cso.customer_name
        , cso.order_no
        , cso.qty_ordered
        , ROW_NUMBER() OVER
          (
              PARTITION BY poh.prod_order_number
              ORDER BY
                    cso.required_date ASC
                  , cso.qty_ordered DESC
                  , cso.customer_name ASC
                  , cso.order_no ASC
          ) AS rn
    FROM prod_order_hdr poh
    INNER JOIN prod_order_line pol
        ON pol.prod_order_number = poh.prod_order_number
    INNER JOIN cte_open_sales_orders cso
        ON cso.inv_mast_uid = pol.inv_mast_uid
    WHERE poh.cancel = 'N'
      AND poh.complete = 'N'
      AND pol.cancel = 'N'
),
cte_target_values AS
(
    SELECT
          rm.prod_order_number
        , rm.required_date
        , rm.customer_name
        , rm.order_no
    FROM cte_ranked_matches rm
    WHERE rm.rn = 1
),
cte_changes AS
(
    SELECT
          tv.prod_order_number
        , tv.required_date AS new_required_date
        , tv.customer_name AS new_customer_name
        , tv.order_no AS sales_order_no
        , poh.required_date AS old_required_date
        , pohud.customer_name AS old_customer_name
    FROM cte_target_values tv
    INNER JOIN prod_order_hdr poh
        ON poh.prod_order_number = tv.prod_order_number
    LEFT JOIN prod_order_hdr_ud pohud
        ON pohud.prod_order_number = tv.prod_order_number
    WHERE
        ISNULL(CAST(poh.required_date AS date), '19000101') <> tv.required_date
        OR ISNULL(LTRIM(RTRIM(pohud.customer_name)), '') <> ISNULL(LTRIM(RTRIM(tv.customer_name)), '')
)

UPDATE poh
SET poh.required_date = c.new_required_date
FROM prod_order_hdr poh
INNER JOIN cte_changes c
    ON c.prod_order_number = poh.prod_order_number;

UPDATE pohud
SET pohud.customer_name = c.new_customer_name
FROM prod_order_hdr_ud pohud
INNER JOIN cte_changes c
    ON c.prod_order_number = pohud.prod_order_number;

SELECT
      c.prod_order_number
    , CAST(c.old_required_date AS date) AS old_required_date
    , CAST(c.new_required_date AS date) AS new_required_date
    , c.old_customer_name
    , c.new_customer_name
    , c.sales_order_no
FROM cte_changes c
ORDER BY c.new_required_date, c.prod_order_number;
