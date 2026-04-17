SET NOCOUNT ON;

WITH cte_open_prod_orders AS
(
    SELECT DISTINCT
          poh.prod_order_number
    FROM prod_order_hdr poh
    INNER JOIN prod_order_line pol
        ON pol.prod_order_number = poh.prod_order_number
    WHERE poh.cancel = 'N'
      AND poh.complete = 'N'
      AND pol.cancel = 'N'
),
cte_eligible_open_sales_orders AS
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
      AND COALESCE(oels.allocated_qty, oel.qty_allocated, 0) <= 0
      AND COALESCE(oels.qty_picked, oel.qty_on_pick_tickets, 0) <= 0
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
    INNER JOIN cte_eligible_open_sales_orders cso
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
        , rm.qty_ordered
    FROM cte_ranked_matches rm
    WHERE rm.rn = 1
)
SELECT
      opo.prod_order_number
    , CAST(pohud.estimated_start_date AS date) AS scheduled_run_date
    , CAST(poh.required_date AS date) AS current_required_date
    , CAST(tv.required_date AS date) AS new_required_date
    , pohud.customer_name AS current_customer_name
    , tv.customer_name AS new_customer_name
    , tv.order_no AS sales_order_no
    , tv.qty_ordered
    , CASE
          WHEN tv.prod_order_number IS NULL THEN 'CLEAR_CUSTOMER_NO_ELIGIBLE_ORDER'
          ELSE 'UPDATE_FROM_ELIGIBLE_ORDER'
      END AS change_reason
FROM cte_open_prod_orders opo
INNER JOIN prod_order_hdr poh
    ON poh.prod_order_number = opo.prod_order_number
LEFT JOIN prod_order_hdr_ud pohud
    ON pohud.prod_order_number = opo.prod_order_number
LEFT JOIN cte_target_values tv
    ON tv.prod_order_number = opo.prod_order_number
WHERE
    (
        tv.prod_order_number IS NOT NULL
        AND
        (
            ISNULL(CAST(poh.required_date AS date), '19000101') <> tv.required_date
            OR ISNULL(LTRIM(RTRIM(pohud.customer_name)), '') <> ISNULL(LTRIM(RTRIM(tv.customer_name)), '')
        )
    )
    OR
    (
        tv.prod_order_number IS NULL
        AND ISNULL(LTRIM(RTRIM(pohud.customer_name)), '') <> ''
    )
ORDER BY
      ISNULL(tv.required_date, CAST(poh.required_date AS date))
    , opo.prod_order_number;
