import React, { useState, useMemo, useEffect } from "react";
import { ListGroup, Form, Badge, Button, Spinner } from "react-bootstrap";
import SearchBar from "./SearchBar";
import SelectedBadgeList from "./SelectedBadgeList";
import { Product } from "@/types/Product";
import s from "./OrderedMultiSelect.module.css";

interface OrderedMultiSelectProps {
    items?: Product[];
    value?: Product[];
    onChange?: (selected: Product[]) => void;
    pageSize?: number;
    endpoint?: string;
    showBadges?: boolean;
}

const OrderedMultiSelect: React.FC<OrderedMultiSelectProps> = ({
                                                                   items = [],
                                                                   value,
                                                                   onChange,
                                                                   pageSize = 10,
                                                                   endpoint,
                                                                   showBadges = true,
                                                               }) => {
    const [selected, setSelected] = useState<Product[]>(value ?? []);
    const [query, setQuery] = useState<string>("");
    const [page, setPage] = useState<number>(1);
    const [remoteItems, setRemoteItems] = useState<Product[]>([]);
    const [totalPages, setTotalPages] = useState<number>(1);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    // Keep external state in sync
    useEffect(() => {
        if (value) setSelected(value);
    }, [value]);

    // Notify parent on selection change
    useEffect(() => {
        onChange?.(selected);
    }, [selected, onChange]);

    // Fetch from endpoint if provided
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

                const res = await fetch(url.toString(), { signal: controller.signal });
                if (!res.ok) throw new Error(`HTTP ${res.status}`);
                const data = await res.json();

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

    // Filter locally if no endpoint is used
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

    useEffect(() => setPage(1), [query]);

    // Always render 10 rows, filling missing with placeholders
    const displayItems = useMemo(() => {
        const filled = [...filteredItems];
        const remaining = pageSize - filled.length;
        if (remaining > 0) {
            for (let i = 0; i < remaining; i++) {
                filled.push(null as never);
            }
        }
        return filled;
    }, [filteredItems, pageSize]);

    return (
        <div className="p-3 border rounded bg-light">
            <SearchBar placeholder="Search products..." onSearch={setQuery} delay={300} />

            {showBadges && (
                <SelectedBadgeList items={selected} onRemove={handleToggle} />
            )}

            <ListGroup className={s.listGroup}>
                {loading ? (
                    <div className="text-center py-4">
                        <Spinner animation="border" size="sm" /> Loading...
                    </div>
                ) : error ? (
                    <ListGroup.Item className="text-danger text-center">{error}</ListGroup.Item>
                ) : (
                    displayItems.map((product, idx) => {
                        if (!product) {
                            return (
                                <ListGroup.Item
                                    key={`placeholder-${idx}`}
                                    className={`text-muted ${s.placeholderItem}`}
                                >
                                    &nbsp;
                                </ListGroup.Item>
                            );
                        }

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
                                <div className={s.productRow}>
                                    <strong className={s.productName}>{product.productName}</strong>
                                    <span className={s.quantity}>
                                        Qty: {product.productQuantity ?? "N/A"}
                                    </span>
                                </div>

                                {isSelected && (
                                    <Badge bg="secondary" pill className={s.badge}>
                                        {index + 1}
                                    </Badge>
                                )}
                            </ListGroup.Item>
                        );
                    })
                )}
            </ListGroup>

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
        </div>
    );
};

export default OrderedMultiSelect;
