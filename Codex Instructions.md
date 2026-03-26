1. Save an overstocked PO without manager approval and confirm approved flips to N.
-- this works
2. Try checking ufc_po_hdr_ud_manager_approved as a non-authorized user and confirm it gets reset.
-- it does not. logged in as a role = PURCHASING and i was allowed to check the box and save.
3. Try the same checkbox as an authorized role and confirm approved can remain checked afterward.
-- this works

we need to do more digging to actually bring in the current role and user of the current session. the sql i sent you is how you check what each user's role is, but it doesn't find the current role and user of the current session.