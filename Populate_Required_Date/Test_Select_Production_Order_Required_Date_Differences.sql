SET NOCOUNT ON;

WITH cte_open_sales_orders AS
(
    SELECT
          oel.inv_mast_uid
        , cst.customer_name
        , CAST(oel.required_date AS date) AS required_date
        , CAST(oel.qty_ordered AS decimal(19, 6)) AS qty_ordered
        , oel.order_no
    FROM oe_line oel
    INNER JOIN oe_hdr oeh
        ON oeh.order_no = oel.order_no
    INNER JOIN customer cst
        ON cst.customer_id = oeh.customer_id
    WHERE oel.complete = 'N'
      AND oel.detail_type <> 1
      AND COALESCE(oeh.order_type, 0) NOT IN (1877, 1706)
      AND ISNULL(oeh.rma_flag, 'N') <> 'Y'
      AND ISNULL(oeh.warranty_rma_flag, 'N') <> 'Y'
      AND oel.required_date IS NOT NULL
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
)
SELECT
      poh.prod_order_number
    , CAST(poh.required_date AS date) AS current_required_date
    , CAST(rm.required_date AS date) AS new_required_date
    , pohud.customer_name AS current_customer_name
    , rm.customer_name AS new_customer_name
    , rm.order_no AS sales_order_no
    , rm.qty_ordered
FROM cte_ranked_matches rm
INNER JOIN prod_order_hdr poh
    ON poh.prod_order_number = rm.prod_order_number
LEFT JOIN prod_order_hdr_ud pohud
    ON pohud.prod_order_number = rm.prod_order_number
WHERE rm.rn = 1
  AND
  (
      ISNULL(CAST(poh.required_date AS date), '19000101') <> rm.required_date
      OR ISNULL(LTRIM(RTRIM(pohud.customer_name)), '') <> ISNULL(LTRIM(RTRIM(rm.customer_name)), '')
  )
ORDER BY rm.required_date, poh.prod_order_number;
