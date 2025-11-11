import React, { useState, useMemo, useEffect } from "react";
import { ListGroup, Form, Badge, Button, Spinner } from "react-bootstrap";
import SearchBar from "./SearchBar";
import { Product } from "@/types/Product";
import s from "./OrderedMultiSelect.module.css";

interface OrderedMultiSelectProps {
    items?: Product[]; // optional when endpoint used
    value?: Product[];
    onChange?: (selected: Product[]) => void;
    pageSize?: number; // default 10
    endpoint?: string; // optional API endpoint for remote pagination
}

/**
 * Paginated, searchable multi-select list.
 * Works with both local (dummy) data and remote API data.
 */
const OrderedMultiSelect: React.FC<OrderedMultiSelectProps> = ({
                                                                   items = [],
                                                                   value,
                                                                   onChange,
                                                                   pageSize = 10,
                                                                   endpoint,
                                                               }) => {
    const [selected, setSelected] = useState<Product[]>(value ?? []);
    const [query, setQuery] = useState<string>("");
    const [page, setPage] = useState<number>(1);
    const [remoteItems, setRemoteItems] = useState<Product[]>([]);
    const [totalPages, setTotalPages] = useState<number>(1);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    // ✅ Sync external control
    useEffect(() => {
        if (value) setSelected(value);
    }, [value]);

    // ✅ Notify parent after commit
    useEffect(() => {
        onChange?.(selected);
    }, [selected, onChange]);

    // ✅ Fetch remote data when endpoint is set
    useEffect(() => {
        if (!endpoint) return;

        const controller = new AbortController();
        const fetchData = async () => {
            setLoading(true);
            setError(null);
            try {
                const url = new URL(endpoint);
                url.searchParams.set("page", String(page));
                url.searchParams.set("limit", String(pageSize));
                if (query.trim()) url.searchParams.set("q", query);

                const res = await fetch(url.toString(), {
                    signal: controller.signal,
                });
                if (!res.ok) throw new Error(`HTTP ${res.status}`);
                const data = await res.json();

                // Expecting { data: Product[], totalPages?: number }
                setRemoteItems(data.data ?? data);
                setTotalPages(data.totalPages ?? 1);
            } catch (err) {
                if (err instanceof DOMException && err.name === "AbortError") return;
                setError("Failed to load products.");
            } finally {
                setLoading(false);
            }
        };

        fetchData();
        return () => controller.abort();
    }, [endpoint, page, query, pageSize]);

    // ✅ Local filtered & paginated mode (dummy data)
    const filteredItems = useMemo(() => {
        if (endpoint) return remoteItems;

        const q = query.toLowerCase();
        const filtered = q
            ? items.filter((p) => p.productName.toLowerCase().includes(q))
            : items;
        setTotalPages(Math.ceil(filtered.length / pageSize));
        const start = (page - 1) * pageSize;
        return filtered.slice(start, start + pageSize);
    }, [items, query, page, pageSize, endpoint, remoteItems]);

    const handleToggle = (product: Product) => {
        setSelected((prev) => {
            const exists = prev.some((p) => p.productId === product.productId);
            return exists
                ? prev.filter((p) => p.productId !== product.productId)
                : [...prev, product];
        });
    };

    const nextPage = () => setPage((p) => Math.min(p + 1, totalPages));
    const prevPage = () => setPage((p) => Math.max(p - 1, 1));

    // Reset to first page on search
    useEffect(() => setPage(1), [query]);

    return (
        <div className="p-3 border rounded bg-light">
            {/* 🔍 Search */}
            <SearchBar placeholder="Search products..." onSearch={setQuery} delay={300} />

            {/* 🎟️ Selected badges */}
            {selected.length > 0 && (
                <div className={`mb-3 d-flex flex-wrap gap-2 ${s.badgeContainer}`}>
                    {selected.map((p, i) => (
                        <Badge
                            key={p.productId}
                            bg="secondary"
                            pill
                            className={s.selectedBadge}
                            title={`Click to remove ${p.productName}`}
                            onClick={() => handleToggle(p)}
                        >
                            {i + 1}. {p.productName}
                        </Badge>
                    ))}
                </div>
            )}

            {/* 📋 Product List */}
            <ListGroup className={s.listGroup}>
                {loading ? (
                    <div className="text-center py-4">
                        <Spinner animation="border" size="sm" /> Loading...
                    </div>
                ) : error ? (
                    <ListGroup.Item className="text-danger text-center">
                        {error}
                    </ListGroup.Item>
                ) : filteredItems.length > 0 ? (
                    filteredItems.map((product) => {
                        const index = selected.findIndex(
                            (p) => p.productId === product.productId
                        );
                        const isSelected = index !== -1;

                        return (
                            <ListGroup.Item
                                key={product.productId}
                                action
                                onClick={(e) => {
                                    e.preventDefault();
                                    handleToggle(product);
                                }}
                                className={`d-flex align-items-center justify-content-between ${
                                    isSelected ? "active" : ""
                                } ${s.listItem}`}
                            >
                                <div className="d-flex align-items-center">
                                    <Form.Check
                                        type="checkbox"
                                        checked={isSelected}
                                        onChange={() => handleToggle(product)}
                                        label={
                                            <>
                                                <strong>{product.productName}</strong>
                                                <span className="text-muted ms-2">
                                                    (Qty: {product.productQuantity})
                                                </span>
                                            </>
                                        }
                                        className={`me-2 ${s.checkbox}`}
                                    />
                                    {isSelected && (
                                        <Badge bg="secondary" pill className={s.badge}>
                                            {index + 1}
                                        </Badge>
                                    )}
                                </div>
                            </ListGroup.Item>
                        );
                    })
                ) : (
                    <ListGroup.Item className={`text-center text-muted ${s.noResults}`}>
                        No products found
                    </ListGroup.Item>
                )}
            </ListGroup>

            {/* 📄 Pagination Controls */}
            {totalPages > 1 && (
                <div className="d-flex justify-content-between align-items-center mt-3">
                    <Button
                        variant="outline-secondary"
                        size="sm"
                        onClick={prevPage}
                        disabled={page === 1 || loading}
                    >
                        ← Previous
                    </Button>
                    <span className="text-muted small">
                        Page {page} of {totalPages}
                    </span>
                    <Button
                        variant="outline-secondary"
                        size="sm"
                        onClick={nextPage}
                        disabled={page === totalPages || loading}
                    >
                        Next →
                    </Button>
                </div>
            )}
        </div>
    );
};

export default OrderedMultiSelect;
